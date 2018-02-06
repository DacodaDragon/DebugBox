using UnityEngine;

namespace ProtoBox.Console.Commands
{
    public class ScreenCommands : ConsoleCommand
    {
        public override string Name { get { return "screen"; } }
        public override string HelpText
        {
            get
            {
                return @"
Available commands:
screen [resolution,res,r] <xRes> <yRes> (fullscreen)
screen [fullscreen,full,f] <fullscreen>
screen fps <targetfps>";
            }
        }

        public override void Run(params string[] args)
        {
            Assert(args.Length == 1, ERR_INVALID_ARG_COUNT);
            
            switch (args[1])
            {
                case "r":
                case "res":
                case "resolution":
                    SetResolution(args);
                    return;

                case "fps":
                    SetTargetFPS(args);
                    return;

                case "f":
                case "full":
                case "fullscreen":
                    SetFullscreen(args);
                    return;
            }

            Fail(ERR_INVALID_SUBCOMMAND);
        }

        private void SetResolution(string[] args)
        {
            Assert(args.Length <= 3, ERR_INVALID_ARG_COUNT);
            int  xres = ParseInt(args[2]);
            int  yres = ParseInt(args[3]);

            if (args.Length == 4)
            {
                Screen.SetResolution(xres, yres, Screen.fullScreen);
                return;
            }

            Screen.SetResolution(xres, yres, ParseBool(args[4]));
        }

        private void SetTargetFPS(string[] args)
        {
            Assert(args.Length <= 2, ERR_INVALID_ARG_COUNT);
            Application.targetFrameRate = ParseInt(args[2]);
        }

        private void SetFullscreen(string[] args)
        {
            Assert(args.Length <= 2, ERR_INVALID_ARG_COUNT);
            Screen.fullScreen = ParseBool(args[2]);
        }
    }

    public class GraphicsCommands : ConsoleCommand
    {
        public override string Name
        {
            get { return "graphics"; }
        }

        public override string HelpText
        {
            get
            {
                return @" List of available commands:
graphics [vsync, sync, v] <bool>
graphics [antialiasing, aa] <int>
graphics [shadowquality, shadow, s] <shadowquality>
graphics [quality, q] <int>
graphics [qualityincrease, qi]
graphics [qualitydecrease, qd]";
            }
        }

        public override void Run(params string[] args)
        {
            Assert(args.Length < 2, ERR_INVALID_ARG_COUNT);
            switch (args[1])
            {
                case "aa":
                case "antialiasing":
                    SetAA(args);
                    return;

                case "sync":
                case "vsync":
                case "v":
                    SetVSync(args);
                    return;

                case "shadow":
                case "shadows":
                case "shadowquality":
                case "s":
                    SetShadowQuality(args);
                    return;

                case "quality":
                case "qualitylevel":
                case "q":
                    SetQualityLevel(args[2]);
                    return;

                case "qualityincrease":
                case "qualityi":
                case "qi":
                    IncreaseQualityLevel();
                    return;

                case "qualitydecrease":
                case "qualityd":
                case "qd":
                    DecreaseQualityLevel();
                    return;
            }

            Fail(ERR_INVALID_SUBCOMMAND);
        }

        private void SetAA(string[] args)
        {
            QualitySettings.antiAliasing = ParseInt(args[2]);
        }

        private void SetVSync(string[] args)
        {
            QualitySettings.antiAliasing = ParseInt(args[2]);
        }

        private void SetShadowQuality(string[] args)
        {
            QualitySettings.shadows = StrToShadowQuality(args[2]);
            
        }

        private void SetAnsio(string[] args)
        {
            QualitySettings.anisotropicFiltering = StrToAniso(args[2]);
            
        }

        private void SetQualityLevel(string args)
        {
            QualitySettings.SetQualityLevel(ParseInt(args), true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private void IncreaseQualityLevel()
        {
            QualitySettings.IncreaseLevel(true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private void DecreaseQualityLevel()
        {
            QualitySettings.DecreaseLevel(true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private ShadowQuality StrToShadowQuality(string arg)
        {
            AssertArg(arg, "disable", "all", "hard");
            switch (arg)
            {
                case "disable": return ShadowQuality.Disable;
                case "all": return ShadowQuality.All;
                case "hard": return ShadowQuality.HardOnly;
                default: return ShadowQuality.Disable;
            }
        }

        private AnisotropicFiltering StrToAniso(string arg)
        {
            AssertArg(arg, "disable", "enable", "force");
            switch (arg)
            {
                case "disable": return AnisotropicFiltering.Disable;
                case "enable": return AnisotropicFiltering.Enable;
                case "force": return AnisotropicFiltering.ForceEnable;
                default: return AnisotropicFiltering.Disable;
            }
        }
    }
}
