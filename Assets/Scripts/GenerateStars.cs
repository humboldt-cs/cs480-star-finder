using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStars : MonoBehaviour
{
    // Star prefab asset
    public GameObject starPrefab;

    private static string star_data_resource = "catalog";
    private const int catalog_entry_size = 197;

    // Start is called before the first frame update
    void Start()
    {
        // Grab file resource
        TextAsset star_data = Resources.Load<TextAsset>(star_data_resource);
        // Get bytes
        byte[] stars = star_data.bytes;

        // Loop through star data and instantiate a prefab for each after coordinate conversion
        for(int i = 0; i < stars.Length; i += catalog_entry_size) 
        {

        }
    }

    // Update is called once per frame
    void Update() 
    {
        // Intentionally empty: stars will be fixed
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
    Vector3 CoordConversion(float right_ascension, float declination, float apparent_magnitude) {
        const float DISTANCE_MIN = 10.0f;
        float distance = (apparent_magnitude + 1) * DISTANCE_MIN; // A more accurate model would be 2.5^apparent magnitude, this is a demonstration

        float x, y, z;

        x = Mathf.Cos(right_ascension) * Mathf.Cos(declination) * distance;
        z = Mathf.Sin(right_ascension) * Mathf.Cos(declination) * distance;
        y = Mathf.Sin(declination) * distance;

        return new Vector3(x, y, z);
    }

    // Conversion from raw RA value to angle in radians for use in sin / cos functions
    float RightAscensionToRadians(float hr, float min, float sec) {
        // Convert hr:min:sec to radians
        float radians = ((hr / 24) + (min / 1440) + (sec / 86400)) * Mathf.PI * 2;

        return radians;
    }

    // Conversion from raw DEC value to angle in radians for use in sin / cos functions
    float DeclinationToRadians(char sign, float deg, float arcmin, float arcsec) {
        // Convert deg:arcminutes:arcseconds to radians
        float radians = ((deg / 360) + (arcmin / 21600) + (arcsec / 1296000)) * Mathf.PI * 2;

        // Convert to negative value if necessary
        if(sign == '-')
        {
            radians *= -1;
        }

        return radians;
    }
}
