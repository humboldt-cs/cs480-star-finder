﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var orientation = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0.0f);
        transform.Rotate(orientation);
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    Vector3 CoordConversion(float right_ascension, float declination)
    {
        //This value is only for making the scene more managable in Unity editor by moving objects farther from the camera
        const float DISTANCE_MOD = 10.0f;

        Vector3 transform;
        float x, y, z;

        x = Mathf.Sin(right_ascension) * DISTANCE_MOD;
        y = Mathf.Cos(right_ascension) * DISTANCE_MOD;
        z = Mathf.Sin(declination) * DISTANCE_MOD;

        transform = new Vector3(x, y, z);

        return transform;
    }
}
