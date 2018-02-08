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

        public override void Execute(params string[] args)
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
        private SubCommand[] m_commands;

        public override string Name { get { return "graphics"; } }

        public override SubCommand[] Commands
        {
            get
            {
                return m_commands;
            }
        }

        private void SetAA(string[] args)
        {
            QualitySettings.antiAliasing = ParseInt(args[2]);
        }

        private void SetVSync(string[] args)
        {
            QualitySettings.vSyncCount = ParseInt(args[2]);
        }

        private void SetShadowQuality(string[] args)
        {
            QualitySettings.shadows = StrToShadowQuality(args[2]);
            
        }

        private void SetAnsio(string[] args)
        {
            QualitySettings.anisotropicFiltering = StrToAniso(args[2]);
            
        }

        private void SetQualityLevel(string[] args)
        {
            QualitySettings.SetQualityLevel(ParseInt(args[2]), true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private void IncreaseQualityLevel(string[] args)
        {
            QualitySettings.IncreaseLevel(true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private void DecreaseQualityLevel(string[] args)
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

        public GraphicsCommands()
        {
            m_commands = new SubCommand[]
            {
                new SubCommand("setaa",                 new string[] { "aa" },                             new Param[] { new Param(typeof(int),           "AntiAliasing level") }, SetAA),
                new SubCommand("setvsync",              new string[] { "vsync", "v" },                     new Param[] { new Param(typeof(int),           "vsync count") }, SetVSync),
                new SubCommand("setshadowquality",      new string[] { "shadowquality", "shadow", "shq" }, new Param[] { new Param(typeof(ShadowQuality), "ShadowQuality") }, SetShadowQuality),
                new SubCommand("setqualitylevel",       new string[] { "qualitylevel", "quality", "q" },   new Param[] { new Param(typeof(int),           "Quality Level") }, SetShadowQuality),
                new SubCommand("increasequalitylevel",  new string[] { "increasequality", "iq" },          null , IncreaseQualityLevel),
                new SubCommand("decreasequalitylevel",  new string[] { "decreasequality", "dq" },          null , DecreaseQualityLevel)
            };
        }
    }
}
