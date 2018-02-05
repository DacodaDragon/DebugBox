using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

[RequireComponent(typeof(Text))]
public class ConsoleText : MonoBehaviour
{
    StringBuilder sb = new StringBuilder();
    Text m_text;
    int index;

    public void Start()
    {
        m_text = GetComponent<Text>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();


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
                if (sb.Length == 0)
                    return;

                sb.Remove(index-1, 1);
                index -= 1;

                if (index < 0)
                    index = 0;

                continue;
            }

            sb.Insert(index++, c);
        }
        m_text.text = sb.ToString();
    }

    public void LateUpdate()
    {
        transform.SetAsLastSibling();
    }

    public void Quit()
    {
        Reset();
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        m_text.text = "";
        sb.Clear();
        index = 0;
    }

    public void Submit()
    {
        if (m_text.text == "")
        {
            gameObject.SetActive(false);
            return;
        }

        Debug.Log(m_text.text);
        m_text.text = "";
        sb.Clear();
        index = 0;
    }

    //public void Update()
    //{
    //
    //}
}
