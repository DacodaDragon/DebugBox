using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

[RequireComponent(typeof(Text))]
public class ConsoleText : MonoBehaviour
{
    private enum CaretDirection
    {
        Left,
        Right
    }

    private const char CARETCHAR = (char)(0x2588);

    private ConsoleCommandHistory m_history = new  ConsoleCommandHistory();
    private StringBuilder m_StringBuilder = new StringBuilder();
    private Text m_TextElement;
    private string m_Command = "";
    private int m_CurrentIndex;
    private bool m_ShowCarret;
    private float m_carretDelay = 0.25f;
    private float m_CarretToggleTime;

    private float m_repeatTime;
    private float m_repeatStartDelay = 0.05f;
    private float m_repeatDelay = 0.025f;
    private bool m_repeating;
    private CaretDirection repeatDirection;

    public void Start()
    {
        m_TextElement = GetComponent<Text>();
        m_TextElement.text = "";
        Quit();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();

        UpdateTextInput();
        UpdateCaret();
        UpdateCaretInput();
        UpdateHistory();
    }

    public void UpdateHistory()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_Command = m_history.GetPrevious();
            ResetCaret();
            ResyncStringBuilder();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_Command = m_history.GetNext();
            ResetCaret();
            ResyncStringBuilder();
        }
    }


    public void ResyncStringBuilder()
    {
        m_StringBuilder = new StringBuilder(m_Command);
    }
    public void UpdateCaretInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            repeatDirection = CaretDirection.Left;
            m_repeatTime = Time.unscaledTime + m_repeatStartDelay;
            HandleCarretInput(repeatDirection);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            repeatDirection = CaretDirection.Right;
            m_repeatTime = Time.unscaledTime + m_repeatStartDelay;
            HandleCarretInput(repeatDirection);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && repeatDirection == CaretDirection.Left)
        {
            if (Time.unscaledTime > m_repeatTime + m_repeatStartDelay + m_repeatDelay)
            {
                m_repeatTime += m_repeatDelay;
                HandleCarretInput(repeatDirection);
            }
        }


        if (Input.GetKey(KeyCode.RightArrow) && repeatDirection == CaretDirection.Right)
        {
            if (Time.unscaledTime > m_repeatTime + m_repeatStartDelay + m_repeatDelay)
            {
                m_repeatTime += m_repeatDelay;
                HandleCarretInput(repeatDirection);
            }
        }
    }

    private void HandleCarretInput(CaretDirection dir)
    {
        if (dir == CaretDirection.Left)
        {
            --m_CurrentIndex;
            if (m_CurrentIndex < 0)
                m_CurrentIndex = 0;
            ResetCaretBlink();
            repeatDirection = CaretDirection.Left;
        }
        else
        {
            ++m_CurrentIndex;
            if (m_CurrentIndex > m_Command.Length)
                m_CurrentIndex = m_Command.Length;
            ResetCaretBlink();
            repeatDirection = CaretDirection.Right;
        }
    }

    public void UpdateCaret()
    {
        // When the carett doesn't have to toggle yet
        if (Time.unscaledTime > m_CarretToggleTime)
        {
            // Toggle the caret
            m_ShowCarret = !m_ShowCarret;

            // If we have to display it
            // show the carret
            if (m_ShowCarret) ShowCarret();

            // Hide the carret when we
            // stop displaying it
            else m_TextElement.text = m_Command;

            // Reset carret blink delay
            m_CarretToggleTime = Time.unscaledTime + m_carretDelay;
        }
    }

    public void ShowCarret()
    {
        StringBuilder displayString = new StringBuilder(m_Command);
        if (m_CurrentIndex < displayString.Length)
        {
            displayString.Remove(m_CurrentIndex, 1).Insert(m_CurrentIndex, CARETCHAR);
            m_TextElement.text = displayString.ToString();
        }
        else
        {
            displayString.Append(CARETCHAR);
            m_TextElement.text = displayString.ToString();
        }
    }

    public void ResetCaretBlink()
    {
        m_CarretToggleTime = 0;
        m_ShowCarret = false;
    }

    public void ResetCaret()
    {
        m_CurrentIndex = m_Command.Length;
        m_CarretToggleTime = 0;
        m_ShowCarret = false;
    }

    public void ForwardCaret()
    {
        m_CurrentIndex = m_Command.Length+1;
        m_CarretToggleTime = 0;
        m_ShowCarret = false;
    }

    public void UpdateTextInput()
    {
        string input = Input.inputString;
        foreach (char c in input)
        {
            if (c == '\n' || c == '\r')
            {
                Submit();
                continue;
            }

            if (c == '\b')
            {
                if (m_StringBuilder.Length == 0)
                    return;

                m_StringBuilder.Remove(m_CurrentIndex - 1, 1);
                m_CurrentIndex -= 1;

                if (m_CurrentIndex < 0)
                    m_CurrentIndex = 0;

                continue;
            }

            m_StringBuilder.Insert(m_CurrentIndex++, c);
        }
        if (input.Length > 0)
        {
            m_Command = m_StringBuilder.ToString();
            m_TextElement.text = m_Command;
            ResetCaretBlink();
        }
    }

    public void LateUpdate()
    {
        transform.SetAsLastSibling();
    }

    public void Quit()
    {
        Reset();
        transform.SetAsFirstSibling();
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        ResetCaret();
        m_TextElement.text = "";
        m_StringBuilder = new StringBuilder();
        m_CurrentIndex = 0;
    }

    public void Submit()
    {
        m_history.Add(m_Command);
        Debug.Log(m_Command);
        Reset();
    }
}

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
        if (m_index != m_history.Count-1)
            ++m_index;
        return command;
    }

    public void Add(string command)
    {
        m_history.Add(command);
        m_index = m_history.Count - 1;
    }
}