using System;
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

        public virtual void Run(params string[] args)
        {
            Fail(string.Format(ERR_NOT_IMPLEMENTED,Name));
        }

        protected void Assert(bool value, string reason)
        {
            if (value) { Fail(reason); }
        }

        protected void Fail(string reason)
        {
            throw new Exception(
                    Name +
                    " command failed: " +
                    reason +
                    (reason[reason.Length - 1] == '.' ? " " : ". ")
                    + string.Format(ERR_END, Name));
        }

        public void PrintHelp()
        {
            if (string.IsNullOrEmpty(HelpText))
                Fail(string.Format(ERR_NOT_IMPLEMENTED,Name));
            Debug.Log(HelpText);
        }

        protected int ParseInt(string value)
        {
            int num;
            Assert(!int.TryParse(value, out num), string.Format(ERR_PARSE_INT, value));
            return num;
        }
        
        protected long ParseLong(string value)
        {
            long num;
            Assert(!long.TryParse(value, out num), string.Format(ERR_PARSE_LON, value));
            return num;
        }

        protected double ParseDouble(string value)
        {
            double num;
            Assert(!double.TryParse(value, out num), string.Format(ERR_PARSE_DOU, value));
            return num;
        }

        protected float ParseFloat(string value)
        {
            float num;
            Assert(float.TryParse(value, out num), string.Format(ERR_PARSE_FLT, value));
            return num;
        }

        protected bool ParseBool(string value)
        {
            bool boolean;
            Assert(!bool.TryParse(value, out boolean), string.Format(ERR_PARSE_BOO, value));
            return boolean;
        }

        protected void AssertArg<T>(T value, params T[] choises)
        {
            if (!choises.Contains(value))
                Fail(string.Format(ERR_INVALID_ARG, value, FormatMultiple(choises)));
        }

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

        public ConsoleCommand()
        {
            HashedName = Name.GetHashCode();
        }
    }
}
