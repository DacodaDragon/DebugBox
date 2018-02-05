using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace tinyBuild.UI
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private Text m_FPS;
        [SerializeField] private Text m_Average;
        [SerializeField] private Text m_Min;
        [SerializeField] private Text m_Max;
        [SerializeField] private Text m_MS;
        [SerializeField] private Text m_TimeStep;

        // Index of min and max to reduce lookup time.
        [SerializeField] private int m_minIndex;
        [SerializeField] private int m_maxIndex;

        [Tooltip("Amount of frames to sample")]
        [SerializeField] private int m_sampleCount;

        private int m_framePointer;
        private List<int> m_FpsList;

        public void Start()
        {
            m_FpsList = new List<int>(m_sampleCount);
        }

        public void Update()
        {
            if (m_FpsList.Count == m_sampleCount)
            {
                // Replace old FPS
                m_FpsList[m_framePointer] = FetchFPS();

                // Update counters
                FetchAndUpdate();

                // Move framepointer
                ++m_framePointer;

                // Wrap framepointer from the end
                // of the array to the beginning
                // of the array.
                if (m_framePointer == m_sampleCount)
                    m_framePointer -= m_sampleCount;
            }
            else
            {
                int fps = FetchFPS();
                m_FpsList.Add(fps);
                m_FPS.text = fps.ToString();
                FetchAndUpdate();
            }
        }

        private void FetchAndUpdate()
        {
            // Fetch data and update text elements
            if (m_minIndex == m_framePointer)
                m_Min.text = FetchMin().ToString();
            if (m_maxIndex == m_framePointer)
                m_Max.text = FetchMax().ToString();
            m_FPS.text = m_FpsList[m_framePointer].ToString();
            m_Average.text = FetchAverage().ToString();
            m_MS.text = FetchMS().ToString("##.##");
            m_TimeStep.text = FetchTimeStep().ToString();
        }

        private int FetchMin()
        {
            int min = m_FpsList.Min();
            m_minIndex = m_FpsList.IndexOf(min);
            return min;
        }

        private int FetchMax()
        {
            int max = m_FpsList.Max();
            m_minIndex = m_FpsList.IndexOf(max);
            return max;
        }

        private int FetchAverage()
        {
            return (int)m_FpsList.Average();
        }

        private int FetchFPS()
        {
            return (int)Mathf.Floor(1 / Time.unscaledDeltaTime);
        }

        private float FetchMS()
        {
            return Time.unscaledDeltaTime * 1000;
        }

        private float FetchTimeStep()
        {
            return Time.timeScale;
        }
    }
}

