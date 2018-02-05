using UnityEngine;
namespace ProtoBox.Console
{
    public class ScreenCommands : ConsoleCommand
    {
        const string ERR_INVALID_ARG_COUNT = "invalid argument count.";

        public override string Name { get { return "screen"; } }

        public override void Run(params string[] args)
        {
            Assert(args.Length == 1, ERR_INVALID_ARG_COUNT);
            
            switch (args[1])
            {
                case "?":
                case "h":
                case "help":
                Debug.Log(@"Available commands:
screen res <xRes> <yRes> (fullscreen)
screen fps <targetfps>
screen fullscreen <fullscreen>");
                return;

                case "r":
                case "res":
                case "resolution":
                    SetResolution(args);
                    return;

                case "fps":
                    SetTargetFPS(args);
                    return;

                case "f":
                case "fullscreen":
                    SetFullscreen(args);
                    return;

                default:
                    Fail("Invalid Subcommand.");
                    return;
            }

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

            Assert(args.Length <= 4, ERR_INVALID_ARG_COUNT);
            bool full = ParseBool(args[4]);
            Screen.SetResolution(xres, yres, full);
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

    //public class SceneCommands : ConsoleCommand
    //{
    //    public override string Name
    //    {
    //        get
    //        {
    //            return "scene";
    //        }
    //    }
    //
    //    //public override void Run(params string[] args)
    //    //{
    //    //    Assert(args.Length == 1, ERR_INVALID_ARG_COUNT);
    //    //    switch (args)
    //    //}
    //}

}
