using UnityEngine;
using UnityEngine.UI;

public delegate void TextUIUpdate();

public class LogMessage : MonoBehaviour
{
    public delegate void LogMessageUpdate(LogMessage context);

    public const float LIFETIME = 4f;
    private const string TIMESTAMP_FORMAT = "hh:mm:ss:ff";
    //public enum TimeFormat
    //{
    //    frames = 10,
    //    timestamp = 20
    //}

    public event TextUIUpdate OnTextUpdated;

    private LayoutElement m_LayoutElement;
    private Text m_textElement;
    private string m_message;
    private string m_formattedMessage;
    private int m_hash = 0;

    private int m_frame = 0;
    private int m_maxframe = 0;
    //private string     m_timestamp = "";
    //private string     m_maxTimestamp = "";
    private float m_time;
    private int m_consecutiveMessages = 0;
    private LogType m_LogType;
    //private TimeFormat m_timeFormat = TimeFormat.frames;
    private bool m_ignoreLifetime = false;
    private bool m_displayed = false;

    public int messageHash { get { return m_hash; } }
    public string message { get { return m_message; } }
    public string formattedMessage { get { return m_formattedMessage; } }
    public int startFrame { get { return m_frame; } }
    public int endFrame { get { return m_frame; } }

    public event LogMessageUpdate OnEnable;
    public event LogMessageUpdate OnDisable;

    //public string    startTime { get { return m_timestamp; } }
    //public string    endTime { get { return m_maxTimestamp; } }

    //public TimeFormat timeFormat
    //{
    //    get
    //    {
    //        return m_timeFormat;
    //    }
    //
    //    set
    //    {
    //        m_timeFormat = value;
    //        m_formattedMessage = Format(m_message, m_LogType);
    //        UpdateText();
    //    }
    //}

    public void Awake()
    {
        gameObject.SetActive(false);
        m_textElement = GetComponent<Text>();
    }

    public void ShowNewMessage(string message, string stack, LogType type)
    {
        m_hash = message.GetHashCode() + stack.GetHashCode();
        m_message = message;
        m_frame = Time.frameCount;
        m_maxframe = Time.frameCount;
        m_time = Time.unscaledTime;
        //m_timestamp = System.DateTime.Now.ToString(TIMESTAMP_FORMAT);
        m_consecutiveMessages = 0;
        m_LogType = type;
        m_formattedMessage = Format(message, type);
        transform.SetAsLastSibling();
        UpdateText();
        gameObject.SetActive(true);

        if (OnEnable != null)
            OnEnable.Invoke(this);
    }

    public void Repeat()
    {
        m_consecutiveMessages++;
        m_maxframe = Time.frameCount;
        //m_maxTimestamp = System.DateTime.Now.ToString(TIMESTAMP_FORMAT);
        m_formattedMessage = Format(m_message, m_LogType);
        UpdateText();
        gameObject.SetActive(true);
        m_time = Time.unscaledTime;
    }

    private string Format(string message, LogType type)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        // Start with aproperiate tag
        switch (type)
        {
            case LogType.Error: builder.Append("<color=#b70e0e>[ERROR] "); break;
            case LogType.Assert: builder.Append("<color=#b70e0e>[ASSERT] "); break;
            case LogType.Exception: builder.Append("<color=#b70e0e>[EXCEPTION] "); break;
            case LogType.Warning: builder.Append("<color=orange>[WARNING] "); break;
            case LogType.Log: builder.Append("<color=white>[LOG] "); break;
        }

        // Add what frame it starts
        // and if necessary when the
        // the log stopped making consecutive messages.
        //if (m_timeFormat == TimeFormat.frames)
        //{
        builder.Append("[frame ").Append(m_frame);
        if (m_consecutiveMessages > 0)
        {
            builder.Append('-').Append(m_maxframe);
        }
        builder.Append("] ");
        //}
        //else
        //{
        //    builder.Append("[time ").Append(m_timestamp);
        //    if (m_consecutiveMessages > 0)
        //    {
        //        builder.Append('-').Append(m_maxTimestamp);
        //    }
        //    builder.Append("] ");
        //}

        // Add how many times the message has been repeated
        if (m_consecutiveMessages > 0)
        {
            builder.Append("[repeated ").Append(m_consecutiveMessages).Append("] ");
        }

        // Finish up, close color tag
        return builder.Append(m_message).Append("</color>").ToString();
    }

    public void ManualUpdate()
    {
        if (m_ignoreLifetime)
            return;
        if (Time.unscaledTime > m_time + LIFETIME)
        {
            gameObject.SetActive(false);

            if (OnEnable != null)
                OnEnable.Invoke(this);
        }
    }

    private void UpdateText()
    {
        m_textElement.text = m_formattedMessage;
        if (OnTextUpdated != null)
            OnTextUpdated();
    }
}
