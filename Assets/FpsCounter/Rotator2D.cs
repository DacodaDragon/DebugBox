using UnityEngine;

public class Rotator2D : MonoBehaviour
{

    [SerializeField]
    bool UseScaledTime = true;
    [SerializeField]
    float RotationDegPerSecond;

    // Update is called once per frame
    void Update()
    {
        float time = GetTime();
        transform.rotation = Quaternion.Euler(0, 0, time * RotationDegPerSecond);
    }

    float GetTime()
    {
        if (UseScaledTime)
            return Time.time;
        else return Time.unscaledTime;
    }
}
