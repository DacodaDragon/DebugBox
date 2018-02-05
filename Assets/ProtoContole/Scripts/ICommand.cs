using System;

namespace ProtoBox.Console
{
    public abstract class ConsoleCommand
    {
        const string ERR_END = "type \"{0} help\" for help";
        const string ERR_INVALID_ARG_COUNT = "invalid argument count.";
        const string ERR_PARSE_INT = "failed parsing \"{0}\" to integer";
        const string ERR_PARSE_FLT = "failed parsing \"{0}\" to float";
        const string ERR_PARSE_DOU = "failed parsing \"{0}\" to double";
        const string ERR_PARSE_LON = "failed parsing \"{0}\" to long";
        const string ERR_PARSE_BOO = "failed parsing \"{0}\" to bool";

        public virtual string Name { get; }
        
        public virtual void Run(params string[] args)
        {

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
    }
}
