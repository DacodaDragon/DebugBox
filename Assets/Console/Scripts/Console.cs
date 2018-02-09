using System.Collections.Generic;
using UnityEngine;
using ProtoBox.Console;

public class Console : MonoBehaviour
{
    [SerializeField] private List<LogMessage> m_logElements;
    [SerializeField] private List<LogMessage> m_activeMessages;

    [SerializeField] bool m_collapse;
    [SerializeField] ConsoleInput m_text;
    private int m_logIndex = 0;

    public void Awake()
    {
        if (FindObjectsOfType<Console>().Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < m_logElements.Count; i++)
        {
            m_logElements[i].OnEnable += RecieveEnabledLog;
            m_logElements[i].OnDisable += RecieveDisabledLog;
        }
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

        // Override unity update function
        // Saves perf mostly on consoles..
        for (int i = 0; i < m_logElements.Count; i++)
        { m_logElements[i].ManualUpdate(); }
    }

    private void RecieveLogMessage(string message, string stacktrace, LogType logtype)
    {
        if (m_collapse)
        {
            if (TryCollapse(message.GetHashCode() + stacktrace.GetHashCode()))
                return;
        }

        DisplayMessage(message, stacktrace, logtype);
    }

    private bool TryCollapse(int hash)
    {
        foreach (LogMessage log in m_logElements)
        {
            if (hash == log.messageHash && log.gameObject.activeInHierarchy)
            {
                log.Repeat();
                return true;
            }
        }
        return false;
    }

    private void DisplayMessage(string message, string stacktrace, LogType logtype)
    {
        m_logElements[m_logIndex].ShowNewMessage(message, stacktrace, logtype);
        ++m_logIndex;
        if (m_logIndex >= m_activeMessages.Count)
            m_logIndex = 0;
    }


    private void RecieveEnabledLog(LogMessage message)
    {
        if (m_activeMessages.Contains(message))
        {
            m_activeMessages.Add(message);
        }
    }

    private void RecieveDisabledLog(LogMessage message)
    {
        if (m_activeMessages.Contains(message))
        {
            m_activeMessages.Remove(message);
        }
    }
}