using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProtoBox.Console.Commands
{
    public class SceneCommand : ConsoleCommand
    {
        public override string Name { get { return "scene"; } }
        public override string HelpText { get { return base.HelpText;  } }

        public override void Execute(params string[] args)
        {
            Assert(args.Length < 2, ERR_INVALID_ARG_COUNT);

            switch (args[1])
            {
                case "r":
                case "restart":
                    Restart();
                    return;
                case "l":
                case "load":
                    Load(args);
                    return;
                case "la":
                case "loadadd":
                    LoadAdd(args);
                    return;
            }

            Fail(ERR_INVALID_SUBCOMMAND);
        }

        private void Load(string[] args)
        {
            Assert(args.Length <= 2, ERR_INVALID_ARG_COUNT);
            SceneManager.LoadScene(ParseInt(args[2]));
        }

        private void LoadAdd(string[] args)
        {
            Assert(args.Length <= 2, ERR_INVALID_ARG_COUNT);
            SceneManager.LoadScene(ParseInt(args[2]),LoadSceneMode.Additive);
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public class QuitCommand : ConsoleCommand
    {
        public override string Name { get { return "quit"; } }
        public override string HelpText { get { return "quits the application"; } }

        public override void Execute(params string[] args)
        {
            Application.Quit();
        }
    }

    public class LogAmount : ConsoleCommand
    {
        public override string Name { get { return "log"; } }
        //public override string HelpText { get { return string.Empty; } }

        public override void Execute(params string[] args)
        {
            Assert(args.Length <= 1, ERR_INVALID_ARG_COUNT);
            switch (args[1])
            {
                case "s":
                case "stack":
                case "stacktrace":
                case "trace":
                    SetTrace(args);
                    return;
            }
            Fail(ERR_INVALID_SUBCOMMAND);
        }

        private void SetTrace(string[] args)
        {
            Assert(args.Length <= 3, ERR_INVALID_ARG_COUNT);
            Application.SetStackTraceLogType(StrToLogType(args[2]), StrToTraceType(args[3]));
        }

        private StackTraceLogType StrToTraceType(string arg)
        {
            AssertArg(arg, "none", "script", "full");

            switch (arg)
            {
                case "none": return StackTraceLogType.None;
                case "full": return StackTraceLogType.Full;
                case "script": return StackTraceLogType.ScriptOnly;
                default: return StackTraceLogType.ScriptOnly;
            }
        }
        
        private LogType StrToLogType(string arg)
        {
            AssertArg(arg, "error", "exception", "log", "warning", "assert");

            switch (arg)
            {
                case "error": return LogType.Error;
                case "exception": return LogType.Exception;
                case "log": return LogType.Log;
                case "warning": return LogType.Warning;
                case "assert": return LogType.Assert;
                default: return LogType.Log;
            }
        }
    }
}

