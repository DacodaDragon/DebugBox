using System.Collections.Generic;
using UnityEngine;
using ProtoBox.Console;

public class Console : MonoBehaviour
{
    [SerializeField] private List<LogMessage> m_messageElements;
    [SerializeField] bool m_collapse;
    [SerializeField] ConsoleInput m_text;
    private int m_logIndex = 0;

    public void Awake()
    {
        if (FindObjectsOfType<Console>().Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    { 
        Application.logMessageReceived += RecieveLogMessage;
        Debug.Log("DACODA CONSOLE. Press \"~\" to open the console. Type help for a list of available commands.");
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.BackQuote))
            m_text.gameObject.SetActive(true);


        if (Input.GetKey(KeyCode.F3))
            for (int i = 0; i < 10; i++)
            {
                Debug.LogError("first error about nullreference");
                Debug.LogError("second error about out of range stuff");
                Debug.LogError("third error wow");
                Debug.LogError("fourth error out of pixel bound idunno");
            }
    }

    private void RecieveLogMessage(string message, string stacktrace, LogType logtype)
    {
        if (m_collapse)
        {
            int hash = message.GetHashCode() + stacktrace.GetHashCode();
            foreach (LogMessage log in m_messageElements)
            {
                if (hash == log.messageHash && log.gameObject.activeInHierarchy)
                {
                    log.Repeat();
                    return;
                }
            }
        }
        
        m_messageElements[m_logIndex].ShowNewMessage(message, stacktrace, logtype);
        ++m_logIndex;
        if (m_logIndex >= m_messageElements.Count)
            m_logIndex = 0;
    }
}
