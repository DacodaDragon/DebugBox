using UnityEngine;

namespace ProtoBox.Console.Commands
{
    public class ScreenCommands : ConsoleCommand
    {
        public override string Name
        { get { return "screen"; } }

        private const string ERR_INVALID_RES = "invalid resolution got {0}x{1}, available resoltions are {2}";
        private void ForceResolution(string[] args)
        {
            Assert(args.Length <= 3, ERR_INVALID_ARG_COUNT);
            int xres = ParseInt(args[2]);
            int yres = ParseInt(args[3]);

            if (args.Length == 4)
            {
                Screen.SetResolution(xres, yres, Screen.fullScreen);
                return;
            }

            Screen.SetResolution(xres, yres, ParseBool(args[4]));
        }

        // TODO: Create different implementations for set resolution console command
        private void SetResolution(string[] args)
        {
            Assert(args.Length <= 3, ERR_INVALID_ARG_COUNT);

            // parse x and y
            int xres = ParseInt(args[2]);
            int yres = ParseInt(args[3]);

            // check if resolution is in the list
            // of available resolutions
            bool success = false;
            Resolution[] resolutions = Screen.resolutions;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (xres == resolutions[i].width && yres == resolutions[i].height)
                {
                    success = true;
                    break;
                }
            }

            Assert(!success, string.Format(ERR_INVALID_RES, xres, yres, FormatMultipleResolutions(resolutions)));

            // if we have have 4 arguments run this
            if (args.Length == 4)
            {
                Screen.SetResolution(xres, yres, Screen.fullScreen);
                return;
            }

            // if we have have more than four arguments
            // treat 5th argument to be fullscreen argument
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

        /// <summary>
        /// formats Resolution[] to "[0x0, 800x600]"
        /// </summary>
        /// <param name="resolutions"></param>
        /// <returns></returns>
        private string FormatMultipleResolutions(Resolution[] resolutions)
        {
            if (resolutions == null || resolutions.Length == 0)
                return "N/A";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append('[');
            sb.Append(resolutions[0].width).Append('x').Append(resolutions[0].height);
            for (int i = 1; i < resolutions.Length; i++)
            {
                sb.Append(", ").Append(resolutions[i].width).Append('x').Append(resolutions[i].height);
            }
            sb.Append(']');

            return sb.ToString();
        }

        public ScreenCommands()
        {
            Commands = new SubCommand[]
            {
                new SubCommand("forceresolution",new string[] {"forceres","fres","fr"},new Param[]{ new Param(typeof(int),"xres"),new Param(typeof(int),"yres")}, ForceResolution),
                //new SubCommand("resolution",new string[] {"setres","res","r"},new Param[]{ new Param(typeof(int),"xres"),new Param(typeof(int),"yres")}, SetResolution),
                new SubCommand("resolution",new string[] {"setres","res","r"},new Param[]{ new Param(typeof(Resolution[]), "resolutions", Screen.resolutions)}, SetResolution),
                new SubCommand("targetfps",new string[] {"targetfps","s"},new Param[]{ new Param(typeof(int),"framerate")}, SetResolution),
                new SubCommand("fullscreen",new string[] {"full","fs"},new Param[]{ new Param(typeof(bool),"fullscreen")}, SetResolution)
            };
        }
    }
}
