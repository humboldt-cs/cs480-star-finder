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
        // Test call of RA->Deg
        float temp = RightAscensionToDegrees(64508.9f);

        // Test star generation data
        // Sirius - RA: 101.2875, Dec: -16.7161
        Instantiate(starPrefab, CoordConversion(101.2875f, -16.7161f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // Intentionally empty: stars will be fixed
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
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

    float RightAscensionToDegrees(float right_ascension)
    {
        // Grab arcseconds from raw data
        float ss = Mathf.Round(right_ascension % 100);

        // Grab arcminutes from raw data
        float mm = Mathf.Floor((right_ascension % 10000) / 100);

        // Grab archours from raw data
        float hh = Mathf.Floor((right_ascension % 1000000) / 10000);

        // Convert hh:mm:ss to degrees
        float radians = ((hh / 24) + (mm / 1440) + (ss / 86400)) * Mathf.PI * 2;

        return radians;
    }

    float DeclinationToDegrees(float declination)
    {
        

        return 0.0f;
    }
}
