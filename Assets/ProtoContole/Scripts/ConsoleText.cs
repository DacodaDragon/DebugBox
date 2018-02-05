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

    private StringBuilder m_StringBuilder = new StringBuilder();
    private Text m_TextElement;
    private string m_Command;
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
            ResetCaret();
            repeatDirection = CaretDirection.Left;
        }
        else
        {
            ++m_CurrentIndex;
            if (m_CurrentIndex > m_Command.Length)
                m_CurrentIndex = m_Command.Length;
            ResetCaret();
            repeatDirection = CaretDirection.Right;
        }
    }

    public void UpdateCaret()
    {
        if (Time.unscaledTime > m_CarretToggleTime)
        {
            m_ShowCarret = !m_ShowCarret;

            if (m_ShowCarret)
            {
                StringBuilder b = new StringBuilder(m_Command);

                if (b.Length > m_CurrentIndex)
                {
                    b.Remove(m_CurrentIndex, 1).Insert(m_CurrentIndex, CARETCHAR);
                    m_TextElement.text = b.ToString();
                }
                else
                {
                    b.Append(CARETCHAR);
                    m_TextElement.text = b.ToString();
                }
            }
            else m_TextElement.text = m_Command;

            m_CarretToggleTime = Time.unscaledTime + m_carretDelay;
        }
    }

    public void ResetCaret()
    {
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
            ResetCaret();
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
        m_TextElement.text = "";
        m_StringBuilder = new StringBuilder();
        m_CurrentIndex = 0;
    }

    public void Submit()
    {
        Debug.Log(m_Command);
        m_TextElement.text = "";
        m_StringBuilder = new StringBuilder();
        m_CurrentIndex = 0;
    }
}
