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
        float distance = Mathf.Pow(2, apparent_magnitude + 1) + DISTANCE_MIN;

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
        float scale = Mathf.Sqrt(10 / Mathf.Pow(2,apparent_magnitude));
        
        return new Vector3(scale, scale, scale); 
    }

    // Conversion from RA values to angle in radians for use in sin / cos functions
    public static float RightAscensionToRadians(float hr, float min, float sec)
    {
        float radians = ((hr / 24) + (min / 1440) + (sec / 86400)) * Mathf.PI * 2;

        return radians;
    }

    // Conversion from DEC values to angle in radians for use in sin / cos functions
    public static float DeclinationToRadians(char sign, float deg, float arcmin, float arcsec)
    {
        float radians = ((deg / 360) + (arcmin / 21600) + (arcsec / 1296000)) * Mathf.PI * 2;

        if(sign == '-')
        {
            radians *= -1;
        }

        return radians;
    }

    // Calculates Local Sidereal Time at given longitude and date / time
    // LST is a measurment of the angle along the celestial equator from the observer's meridian to the celestial meridian, or 0h RA
    public static float LocalSiderealTime(float longitude, System.DateTime dt)
    {
        // Algorithm taken from Astronomical Algorithms by Jean Meeus

        const double JULIAN_EPOCH = 2451545.0;

        dt = dt.ToUniversalTime();

        double current_julian_day = JulianDay(dt);
        double hour_fraction = dt.Hour / 24.0 + dt.Minute / 1440.0 + dt.Second / 86400.0;
        double adjusted_jd = current_julian_day + hour_fraction;

        double delta_j = (current_julian_day - JULIAN_EPOCH) / 36525;

        double rotation = 280.46061837 +
                          360.98564736629 * (adjusted_jd - JULIAN_EPOCH) +
                          0.000387933 * System.Math.Pow(delta_j, 2) -
                          System.Math.Pow(delta_j, 3) / 38710000;

        double greenwich_sidereal_time = rotation % 360;

        return (float)greenwich_sidereal_time + longitude;
    }

    // Calculates the Julian Day of the given Gregorian Date
    // Will return a JD ending in 0.5, as JD's start at 12:00 UTC
    public static float JulianDay(System.DateTime dt)
    {
        // Algorithm taken from Astronomical Algorithms by Jean Meeus

        int year = dt.Year;
        int month = dt.Month;
        int day = dt.Day;

        if (month < 3)
        {
            year -= 1;
            month += 12;
        }

        int a = year / 100;
        int b = a / 4;
        int c = 2 - a + b;
        int e = (int)(365.25f * (year + 4716));
        int f = (int)(30.6001f * (month + 1));

        float julian_day = c + day + e + f - 1524.5f;

        return julian_day;
    }
}
