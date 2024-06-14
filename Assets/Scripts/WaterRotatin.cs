using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRotatin : MonoBehaviour
{
    [SerializeField]float rotationSpeed = 10f;

    void Update()
    {
        RotateWater();
    }

    void RotateWater()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
