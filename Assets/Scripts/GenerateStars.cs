using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStars : MonoBehaviour
{
    // Star prefab asset
    public GameObject starPrefab;

    // Start is called before the first frame update
    void Start() {
        // Grab CSV data from text file
        TextAsset star_data = Resources.Load<TextAsset>("orion");
        // Split text data by newline
        string[] stars = star_data.text.Split('\n');

        // Loop through star data and instantiate a prefab for each after coordinate conversion
        for(int i = 1; i < stars.Length - 1; i++) {
            // Split current star data into comma deliminated substrings
            string[] current_star = stars[i].Split(',');

            // Attempt to grab coordinate data TODO: surround in try/catch to handle exceptions if strings cannot be parsed
            float right_ascension;
            float.TryParse(current_star[1], out right_ascension);
            float declination;
            float.TryParse(current_star[2], out declination);

            // Convert coordinate data to radians
            right_ascension = RightAscensionToRadians(right_ascension);
            declination = DeclinationToRadians(declination);

            // Instantiate star with name
            GameObject star = Instantiate(starPrefab, CoordConversion(right_ascension, declination), Quaternion.identity);
            star.name = current_star[0];
        }
    }

    // Update is called once per frame
    void Update() {
        // Intentionally empty: stars will be fixed
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
    Vector3 CoordConversion(float right_ascension, float declination) {
        const float DISTANCE_MOD = 10.0f; // This value is only for making the scene more managable in Unity editor by moving objects farther from the camera
        Vector3 transform;
        float x, y, z;

        x = Mathf.Cos(right_ascension) * DISTANCE_MOD;
        y = Mathf.Sin(declination) * DISTANCE_MOD;
        z = Mathf.Sin(right_ascension) * DISTANCE_MOD;

        transform = new Vector3(x, y, z);

        return transform;
    }

    // Conversion from raw RA value to angle in radians for use in sin / cos functions
    float RightAscensionToRadians(float ra_raw) {
        float ra_corrected = Mathf.Abs(ra_raw);

        // Grab seconds from raw data and round to nearest second
        float ss = Mathf.Round(ra_corrected % 100);

        // Grab minutes from raw data
        float mm = (int)((ra_corrected % 10000) / 100);

        // Grab archours from raw data
        float hh = (int)((ra_corrected % 1000000) / 10000);

        // Convert hh:mm:ss to radians
        float radians = ((hh / 24) + (mm / 1440) + (ss / 86400)) * Mathf.PI * 2;

        // Convert to negative value if necessary
        if(ra_raw < 0)
        {
            radians *= -1;
        }

        return radians;
    }

    // Conversion from raw DEC value to angle in radians for use in sin / cos functions
    float DeclinationToRadians(float dec_raw) {
        float dec_corrected = Mathf.Abs(dec_raw);

        // Grab arcseconds from raw data and round to nearest arcsecond
        float ss = Mathf.Round(dec_corrected % 100);

        // Grab arcminutes from raw data
        float mm = (int)((dec_corrected % 10000) / 100);

        // Grab degrees from raw data
        float degrees = (int)((dec_corrected % 1000000) / 10000);

        // Convert deg:arcminutes:arcseconds to radians
        float radians = ((degrees / 360) + (mm / 21600) + (ss / 1296000)) * Mathf.PI * 2;

        // Convert to negative value if necessary
        if(dec_raw < 0)
        {
            radians *= -1;
        }

        return radians;
    }
}
