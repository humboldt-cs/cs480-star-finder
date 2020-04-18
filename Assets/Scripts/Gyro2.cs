using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro2 : MonoBehaviour
{
    private Quaternion initialRotation;
    private Quaternion gyroInitialRotation;

    void Start()
    {
        Input.gyro.enabled = true;

        //initialRotation = transform.rotation;
        //gyroInitialRotation = Input.gyro.attitude;
    }

    void Update()
    {
        //v1: transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);
        //v2: 
        transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
        //v3:
        //Quaternion offsetRotation = Quaternion.Inverse(gyroInitialRotation) * Input.gyro.attitude;
        //transform.rotation = initialRotation * offsetRotation;
    }
}
