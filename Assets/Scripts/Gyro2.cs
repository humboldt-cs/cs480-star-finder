﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro2 : MonoBehaviour
{

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        //transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);
        transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
    }
}
