using System;

using System.Linq;

namespace ProtoBox.Console.Commands
{
    public class SubCommand
    {
        public delegate void CommandMethodDelegate(string[] args);

        public string name { get; private set; }
        public string[] subnames { get; private set; }
        public Param[] parameters { get; private set; }
        private event CommandMethodDelegate execFunc;

        public bool CheckCommand(string arg)
        {
            return (name == arg || subnames.Contains(arg));
        }

        public string GetFormattedCommand(string baseName)
        {
            // Start stringbuilder with basename (First command argument)
            System.Text.StringBuilder sb = new System.Text.StringBuilder(baseName);


            // Space
            sb.Append(' ');


            // Add Names + command names if necessary "["Name", "subname", "subname"]" or "name" 
            if (subnames != null && subnames.Length != 0)
            {
                sb.Append("[\"").Append(name);

                for (int i = 0; i < subnames.Length; i++)
                {
                    sb.Append("\", \"").Append(subnames[i]);
                }

                sb.Append("\"]");
            }
            else sb.Append(name);

            // Space
            sb.Append(' ');

            // add parameters afterwards "<int:id> <string:name> <float:toime>"
            if (parameters != null)
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(parameters[i].type.Name).Append(' ');
            }

            // return formatted commandhelp
            return sb.ToString();
        }

        public void Execute(string[] args)
        {
            
            if (execFunc == null)
                throw new Exception("Subcommand not found!");
            execFunc.Invoke(args);
        }

        public SubCommand(string name, string[] subnames, Param[] parameters, CommandMethodDelegate func)
        {
            this.name = name;
            this.subnames = subnames;
            this.parameters = parameters;
            this.execFunc = func;
        }
    }

    public class Param
    {
        public string Name { get; private set; }
        public bool isFloat { get; private set; }
        public bool isBool { get; private set; }
        public bool isString { get; private set; }
        public bool isEnum { get; private set; }

        public Type type;

        public string FormattedString()
        {
            return "<" + type.Name + ":" + Name + ">";
        }

        public Param(Type type, string name)
        {
            isFloat = (type == typeof(float));
            isString = (type == typeof(string));
            isBool = (type == typeof(bool));
            isEnum = (type == typeof(Enum));
            this.type = type;

            if (isFloat || isString || isBool || isEnum)
            {
                throw new Exception("Invalid type \"" + type.Name + "\" in \"" + name + "\"");
            }
        }
    }
}
