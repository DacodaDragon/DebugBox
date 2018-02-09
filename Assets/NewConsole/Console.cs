using UnityEngine;
using ProtoBox.Console;

namespace ProtoBox.NewConsole
{
    public delegate void Notify<t>(t value);
    public delegate void MessageNotification(string message, string stack,  LogType type);

    public class Console : MonoBehaviour
    {
        private CommandHandler m_commandhandler;
        public event MessageNotification recieveLogMessage;

        private void Awake()
        {
            Application.logMessageReceived += RecieveLogMessage;
            m_commandhandler = new CommandHandler();
            m_commandhandler.Load();
        }

        private void RecieveLogMessage(string message, string stacktrace, LogType type)
        {
            if (recieveLogMessage != null)
                recieveLogMessage.Invoke(message, stacktrace, type);
        }

        //public Param GetParam(string[] arguments, int index)
        //{
        //    if (index == 0)
        //    {
        //        Param param = new Param(typeof(string[]), "CommandNames", m_commandhandler.GetCommandNames());
        //    }
        //
        //
        //    if (index == 1)
        //    {
        //        if (arguments[0] == "help" || arguments[0] == "h" || arguments[0] == "?")
        //            return new Param(typeof(string[]), "CommandNames", m_commandhandler.GetCommandNames());
        //        else return new Param(typeof(string[]), "SubCommandNames", m_commandhandler.GetSubCommandNames(arguments[0]));
        //    }
        //    if (index > 1)
        //        return m_commandhandler.GetParam(arguments[0], arguments[1], arguments.Length - 1);
        //    return null;
        //}

        public void SubmitCommand(string command)
        {
            try
            {
                m_commandhandler.Submit(command);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}

