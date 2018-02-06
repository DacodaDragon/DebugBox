using System.Collections.Generic;
namespace ProtoBox.Console
{
    public class ConsoleCommandHistory
    {
        List<string> m_history = new List<string>();
        int m_index = 0;

        public string GetPrevious()
        {
            if (m_history.Count == 0)
                return "";

            string command = m_history[m_index];
            if (m_index != 0) --m_index;
            return command;
        }

        public string GetNext()
        {
            if (m_history.Count == 0)
                return "";

            string command = m_history[m_index];
            if (m_index != m_history.Count - 1)
                ++m_index;
            return command;
        }

        public void Add(string command)
        {
            if (m_history.Count == 0)
                m_history.Add(command);
            if (m_history[m_history.Count-1] != command)
                m_history.Add(command);
            m_index = m_history.Count - 1;
        }
    }
}
