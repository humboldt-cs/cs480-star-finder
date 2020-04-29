using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StarMath
{
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

    public static Vector3 getRotation(float latitude, float longitude, System.DateTime current_dt)
    {
        const int JULIAN_EPOCH = 2451545;

        // Convert current DateTime to UTC
        System.DateTime dt = current_dt.ToUniversalTime();

        // Convert current date to Julian Day Number and find timespan from J2000.0 reference Julian Day
        int current_julian_day = StarMath.getJulianDay(dt);
        int julian_day_difference = current_julian_day - JULIAN_EPOCH;

        // Add current time of day to rotation value using fractional hour
        float current_hour = dt.Hour + dt.Minute / 60 + dt.Second / 3600;

        // Calculate given rotation for each day / hour that has passed since J2000.0
        float day_rotation = julian_day_difference % 360;   // 360 degrees in a day
        float hour_rotation = current_hour * 15;            // 15 degrees in an hour

        // Apply all rotation values
        double y_rotation = day_rotation + hour_rotation + longitude;

        Vector3 rotation = new Vector3(latitude - 90.0f, (float)y_rotation, 0);

        return rotation;
    }

    public static int getJulianDay(System.DateTime dt)
    {
        // Algorithm written by Bill Jeffries
        // Source: https://quasar.as.utexas.edu/BillInfo/JulianDatesG.html
        // Variable names copied from source, they are not significant

        int year = dt.Year;
        int month = dt.Month;
        int day = dt.Day;

        if (dt.Month == 1 || dt.Month == 2)
        {
            year -= 1;
            month += 12;
        }

        int a = year / 100;
        int b = a / 4;
        int c = 2 - a + b;
        int e = Mathf.FloorToInt(365.25f * (year + 4716));
        int f = Mathf.FloorToInt(30.6001f * (month + 1));

        int julian_day = Mathf.FloorToInt(c + day + e + f - 1524.5f);

        return julian_day;
    }
}
