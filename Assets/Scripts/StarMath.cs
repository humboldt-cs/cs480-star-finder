using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StarMath
{
    private const float DRAW_DISTANCE = 1000f;

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
    public static Vector3 CoordConversion(float right_ascension, float declination, float apparent_magnitude) {
        const float DISTANCE_MIN = 20.0f;
        float distance = Mathf.Pow(2, apparent_magnitude) + DISTANCE_MIN;

        float x, y, z;

        x = Mathf.Cos(right_ascension) * Mathf.Cos(declination) * distance;
        z = Mathf.Sin(right_ascension) * Mathf.Cos(declination) * distance;
        y = Mathf.Sin(declination) * distance;

        return new Vector3(x, y, z);
    }

    // alternate coordinate conversion without use of apparent magnitude
    public static Vector3 CoordConversion(float right_ascension, float declination)
    {
        float x, y, z;

        x = Mathf.Cos(right_ascension) * Mathf.Cos(declination) * DRAW_DISTANCE;
        z = Mathf.Sin(right_ascension) * Mathf.Cos(declination) * DRAW_DISTANCE;
        y = Mathf.Sin(declination) * DRAW_DISTANCE;

        return new Vector3(x, y, z);
    }

    // Conversion from apparent magnitude to star scale vector
    public static Vector3 ScaleFactor(float apparent_magnitude)
    {
        float scale = 10 / (apparent_magnitude + 2);
        // adjust for Sirius, it's too big
        if (scale > 15f) { scale /= 3; }
        return new Vector3(scale, scale, scale);
        
    }

    // Conversion from RA values to angle in radians for use in sin / cos functions
    public static float RightAscensionToRadians(float hr, float min, float sec) {
        // Convert hr:min:sec to radians
        float radians = ((hr / 24) + (min / 1440) + (sec / 86400)) * Mathf.PI * 2;

        return radians;
    }

    // Conversion from DEC values to angle in radians for use in sin / cos functions
    public static float DeclinationToRadians(char sign, float deg, float arcmin, float arcsec) {
        // Convert deg:arcminutes:arcseconds to radians
        float radians = ((deg / 360) + (arcmin / 21600) + (arcsec / 1296000)) * Mathf.PI * 2;

        // Make dec negative if appropriate
        if(sign == '-')
        {
            radians *= -1;
        }

        return radians;
    }
}
