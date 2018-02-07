using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField]
    float DirectionalSpeed;
    [SerializeField]
    float RotationalSpeed;
    [SerializeField]
    Vector3 Blep;

    private float s = 0;

    void Update()
    {
        s += Mathf.Deg2Rad * (RotationalSpeed * Time.deltaTime);
        float x = (Mathf.Sin(Blep.x * s) * Time.deltaTime) * DirectionalSpeed;
        float y = (Mathf.Sin(Blep.y * s) * Time.deltaTime) * DirectionalSpeed;
        float z = (Mathf.Sin(Blep.z * s) * Time.deltaTime) * DirectionalSpeed;
        transform.Rotate(x, y, z);
    }
}
