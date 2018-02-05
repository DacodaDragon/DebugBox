using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace ProtoBox.Console
{
    public class CommandHandler
    {
        ConsoleCommand[] commands;
        public void Load()
        {
            commands = GetEnumerableOfType<ConsoleCommand>().ToArray();
        }

        public void Submit(string command)
        {
            if (!TryParse(command))
            {
                throw new Exception("Uknown Command: " + command.Split(' ')[0]);
            }
        }

        private bool TryParse(string command)
        {
            string[] tokens = command.Split(' ');
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].Name == tokens[0])
                {
                    commands[i].Run(tokens);
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            objects.Sort();
            return objects;
        }
    }
}
