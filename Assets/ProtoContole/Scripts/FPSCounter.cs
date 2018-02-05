using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FPSCounter : MonoBehaviour
{
    private int m_framePointer;
    [SerializeField] Text m_FPS;
    [SerializeField] Text m_Average;
    [SerializeField] Text m_Min;
    [SerializeField] Text m_Max;

    [SerializeField] private int m_testFrameCount;
    List<int> FPS = new List<int>();

    public void Update()
    {
        if (FPS.Count == m_testFrameCount)
        {
            FPS[m_framePointer] = (int)(1 / Time.unscaledDeltaTime);
            ++m_framePointer;
            if (m_framePointer == m_testFrameCount)
                m_framePointer -= m_testFrameCount;
        }
        else FPS.Add((int)(1 / Time.unscaledDeltaTime));
        m_FPS.text =     "FPS    : " + ((int)(1 / Time.unscaledDeltaTime)).ToString();
        m_Average.text = "Average: " + ((int)FPS.Average()).ToString();
        m_Min.text =     "Min    : " + FPS.Min().ToString();
        m_Max.text =     "Max    : " + FPS.Max().ToString();
    }
}
