using System;
using System.Collections.Generic;
using ProtoBox.Console.Commands;
using UnityEngine;

namespace ProtoBox.Console
{
    public class CommandHandler
    {
        private const string ERROR_UNKNOWN_COMMAND = "Uknown command: \"{0}\". type \"help\" for a list of available commands.";

        List<Commands.ConsoleCommand> commands = new List<ConsoleCommand>();
        public void Load()
        {
            commands.Add(new QuitCommand());
            commands.Add(new ScreenCommands());
            commands.Add(new GraphicsCommands());
            commands.Add(new SceneCommand());
            commands.Add(new TimeCommands());
        }

        public void Submit(string command)
        {
            if (!TryParse(command))
            {
                throw new Exception(string.Format(ERROR_UNKNOWN_COMMAND, command));
            }
        }

        private bool TryParse(string command)
        {
            string[] tokens = command.Split(' ');

            switch (tokens[0])
            {
                case "help":
                case "?":
                case "h":
                    if (tokens.Length == 2)
                    {
                        for (int i = 0; i < commands.Count; i++)
                        {
                            if (commands[i].Name == tokens[1])
                            {
                                commands[i].PrintHelp();
                                return true;
                            }
                        }
                    }
                    Debug.Log("\nList of present commands:\n" + GetFormattedCommandList());
                    return true;
            }

            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].Name == tokens[0])
                {
                    commands[i].Execute(tokens);
                    return true;
                }
            }
            return false;
        }

        public string GetFormattedCommandList()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append('[').Append(commands[0].Name);
            for (int i = 1; i < commands.Count; i++)
            {
                sb.Append(", ").Append(commands[i].Name);
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
