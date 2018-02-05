using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    [SerializeField] private List<LogMessage> m_messageElements;
    [SerializeField] bool m_collapse;
    [SerializeField] ConsoleText m_text;
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
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.BackQuote))
            m_text.gameObject.SetActive(true);

        if (Input.GetKey(KeyCode.F1))
            Debug.LogError("sdfhgusdofgnh osufmhguiomfdsh guiosdfmhg usiofdmgh udsiofmg hufdsiomg hudsfiomg hsuiofdgm hsudio");
    }

    private void RecieveLogMessage(string message, string stacktrace, LogType logtype)
    {
        if (m_collapse)
        foreach (LogMessage log in m_messageElements)
        {
            if (message.GetHashCode() == log.messageHash && log.gameObject.activeInHierarchy)
            {
                log.Repeat();
                return;
            }
        }
        m_messageElements[m_logIndex].ShowNewMessage(message, logtype);
        ++m_logIndex;
        if (m_logIndex >= m_messageElements.Count)
            m_logIndex = 0;
    }
}
