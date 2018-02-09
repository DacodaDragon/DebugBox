//using System.Collections.Generic;
//using UnityEngine;
//using Param = ProtoBox.Console.Commands.Param;

//namespace ProtoBox.NewConsole.Input
//{
//    public class ConsoleInput : MonoBehaviour
//    {
//        [SerializeField]
//        Console m_console;
//        private List<string> m_arguments;
//        private int m_currentArgument;
//        private int m_currentCharIndex;
//        public event Notify<Param> OnArgumentChange;

//        public void NextArgument()
//        {
//            if (m_arguments[m_currentArgument].Trim() == "")
//                return;

//            if (m_arguments.Count == m_currentArgument)
//                m_arguments.Add("");

//            m_currentArgument++;

//            if (OnArgumentChange != null)
//                OnArgumentChange.Invoke(m_console.GetParam(m_arguments.ToArray(), m_currentArgument + 1));
//        }

//        public void PreviousArgument()
//        {
//            if (m_currentArgument - 1 < 0)
//                return;

//            --m_currentArgument;

//            if (OnArgumentChange != null)
//                OnArgumentChange.Invoke(m_console.GetParam(m_arguments.ToArray(), m_currentArgument + 1));
//        }

//        public void ReplaceArgument(string value)
//        {

//        }

//        public void InsertCharacter()
//        {

//        }

//        private void Reset()
//        {
//            m_arguments.Clear();
//            m_arguments.Add("");
//            m_currentArgument = 0;
//            m_currentCharIndex = 0;
//        }
//    }

//    public enum CursorDirection
//    {
//        Left,
//        Right
//    }
//}

