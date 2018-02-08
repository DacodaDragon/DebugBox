﻿using System;
using System.Linq;
using UnityEngine;

namespace ProtoBox.Console.Commands
{
    public abstract class ConsoleCommand
    {
        protected const string ERR_END = "type \"help {0}\" for help";

        protected const string ERR_INVALID_ARG_COUNT = "invalid argument count";
        protected const string ERR_INVALID_ARG = "invalid argument, got \"{0}\" expected \"{1}\"";
        protected const string ERR_INVALID_SUBCOMMAND = "invalid subcommand.";

        protected const string ERR_NOT_IMPLEMENTED = "{0} command has not been implemented";
        protected const string ERR_PARSE_INT = "failed parsing \"{0}\" to integer";
        protected const string ERR_PARSE_FLT = "failed parsing \"{0}\" to float";
        protected const string ERR_PARSE_DOU = "failed parsing \"{0}\" to double";
        protected const string ERR_PARSE_LON = "failed parsing \"{0}\" to long";
        protected const string ERR_PARSE_BOO = "failed parsing \"{0}\" to bool";

        public virtual string Name { get; }
        public virtual int HashedName { get; protected set; }
        public virtual string HelpText { get; }
        public virtual SubCommand[] Commands { get; }


        /// <summary>
        /// Executes commands
        /// </summary>
        /// <param name="args">command arguments</param>
        public virtual void Execute(params string[] args)
        {
            if (TryExecuteSubcommand(args))
                return;

            Fail(string.Format(ERR_NOT_IMPLEMENTED, Name));
        }

        /// <summary>
        /// Tries to find and execute a command with the arguments given
        /// </summary>
        /// <param name="args">arguments</param>
        /// <returns>success</returns>
        private bool TryExecuteSubcommand(string[] args)
        {
            if (Commands == null)
                return false;

            Assert(args.Length <= 1, ERR_INVALID_ARG_COUNT);
            for (int i = 0; i < Commands.Length; i++)
            {
                if (Commands[i].CheckCommand(args[1]))
                {
                    Commands[i].Execute(args);
                    return true;
                }
            }
            Fail(ERR_INVALID_SUBCOMMAND);
            return false;
        }

        /// <summary>
        /// Throws exception depending on value
        /// </summary>
        /// <param name="value">value to check on</param>
        /// <param name="reason">reason of failure when check doesn't pass</param>
        protected void Assert(bool value, string reason)
        {
            if (value) { Fail(reason); }
        }

        /// <summary>
        /// Throw formatted exception 
        /// </summary>
        /// <param name="reason">reason for failure</param>
        protected void Fail(string reason)
        {
            throw new Exception(
                    Name +
                    " failed: " +
                    reason +
                    (reason[reason.Length - 1] == '.' ? " " : ". ")
                    + string.Format(ERR_END, Name));
        }

        /// <summary>
        /// Prints a list of subcommands if available or reverts to helptext
        /// </summary>
        public void PrintHelp()
        {
            if (Commands != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("List of available commands:\n");

                for (int i = 0; i < Commands.Length; i++)
                {
                    sb.Append(Commands[i].GetFormattedCommand(Name)).Append('\n');
                }
                Debug.Log(sb.ToString());
                return;
            }

            if (string.IsNullOrEmpty(HelpText))
                Fail(string.Format(ERR_NOT_IMPLEMENTED, Name));
            Debug.Log(HelpText);

        }

        /// <summary>
        /// parses string to int
        /// </summary>
        /// <param name="value">string to parse</param>
        /// <returns>parsed integer</returns>
        protected int ParseInt(string value)
        {
            int num;
            Assert(!int.TryParse(value, out num), string.Format(ERR_PARSE_INT, value));
            return num;
        }

        /// <summary>
        /// parses string to long
        /// </summary>
        /// <param name="value">string to parse</param>
        /// <returns>parsed long</returns>
        protected long ParseLong(string value)
        {
            long num;
            Assert(!long.TryParse(value, out num), string.Format(ERR_PARSE_LON, value));
            return num;
        }
        /// <summary>
        /// parses string to double
        /// </summary>
        /// <param name="value">string to parse</param>
        /// <returns>parsed double</returns>
        protected double ParseDouble(string value)
        {
            double num;
            Assert(!double.TryParse(value, out num), string.Format(ERR_PARSE_DOU, value));
            return num;
        }

        /// <summary>
        /// parses string to float
        /// </summary>
        /// <param name="value">string to parse</param>
        /// <returns>parsed float</returns>
        protected float ParseFloat(string value)
        {
            float num;
            Assert(!float.TryParse(value, out num), string.Format(ERR_PARSE_FLT, value));
            return num;
        }

        /// <summary>
        /// parses string to boolean
        /// </summary>
        /// <param name="value">string to parse</param>
        /// <returns>boolean</returns>
        protected bool ParseBool(string value)
        {
            bool boolean;
            Assert(!bool.TryParse(value, out boolean), string.Format(ERR_PARSE_BOO, value));
            return boolean;
        }

        /// <summary>
        /// Checks if value is one of the choices given
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="value">value to check with</param>
        /// <param name="choises">choices</param>
        protected void AssertArg<T>(T value, params T[] choises)
        {
            if (!choises.Contains(value))
                Fail(string.Format(ERR_INVALID_ARG, value, FormatMultiple(choises)));
        }

        /// <summary>
        /// Formats multiple values formatted string "[value1, value2, value3]"
        /// </summary>
        /// <typeparam name="T">Type of arguments</typeparam>
        /// <param name="args">arguments to list</param>
        /// <returns>formatted string</returns>
        private string FormatMultiple<T>(T[] args)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(args[0].ToString());
            for (int i = 1; i < args.Length; i++)
            {
                sb.Append(", ").Append(args[i].ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleCommand()
        {
            HashedName = Name.GetHashCode();
        }
    }


    public class TimeCommands : ConsoleCommand
    {
        private SubCommand[] m_SubCommands;
        public override string Name
        { get { return "time"; } }

        public override SubCommand[] Commands
        { get { return m_SubCommands; } }

        private void SetTimeScale(string[] args)
        {
            Assert(args.Length < 3, ERR_INVALID_ARG_COUNT);
            Time.timeScale = ParseFloat(args[2]);
        }

        public TimeCommands()
        {
            m_SubCommands = new SubCommand[] {
                new SubCommand(
                    "scale", 
                    new string[] { "s", "sc" }, 
                    new Param[] { new Param(typeof(int), "time scale") },
                    SetTimeScale)
            };
        }
    }
}
