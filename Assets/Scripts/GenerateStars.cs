using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStars : MonoBehaviour
{
    // Placeholder for Sirius
    public GameObject starPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Test star generation data
        // Sirius - RA: 101.2875, Dec: -16.7161
        Instantiate(starPrefab, CoordConversion(101.2875f, -16.7161f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in degrees
    Vector3 CoordConversion(float right_ascension, float declination)
    {
        const float DISTANCE_MOD = 10.0f; // This value is only for making the scene more managable in Unity editor by moving objects farther from the camera
        Vector3 transform;
        float x, y, z;

        x = Mathf.Sin(right_ascension) * DISTANCE_MOD;
        y = Mathf.Cos(right_ascension) * DISTANCE_MOD;
        z = Mathf.Sin(declination) * DISTANCE_MOD;

        transform = new Vector3(x, y, z);

        return transform;
    }
}
