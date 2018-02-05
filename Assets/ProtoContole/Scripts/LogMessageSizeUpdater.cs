using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogMessageSizeUpdater : MonoBehaviour
{
    private Text m_text;
    private RectTransform m_textTransform;
    private RectTransform m_parentTransform;

    private void Start()
    {
        m_text = GetComponent<Text>();
        m_textTransform = m_text.GetComponent<RectTransform>();
        m_parentTransform = transform.parent.GetComponent<RectTransform>();
        GetComponent<LogMessage>().OnTextUpdated += ResetSize;
        ResetSize();
    }

    private void ResetSize()
    {
        m_textTransform.sizeDelta = new Vector2(m_parentTransform.rect.width, m_text.preferredHeight + 2f);
        m_textTransform.sizeDelta = new Vector2(m_parentTransform.rect.width, m_text.preferredHeight + 2f);

    }
}
