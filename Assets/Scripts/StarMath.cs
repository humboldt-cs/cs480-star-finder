using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StarMath
{
    const double JULIAN_EPOCH = 2451545.0;
    private const float DRAW_DISTANCE = 1000f;

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expects RA/Dec values to be in radians
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

    // Mathematical mod function intended to replace C# remainder operator in angle calculations
    public static double AngleMod(double a, double b)
    {
        if (a < 0)
        {
            return b - (System.Math.Abs(a) % b);
        }
        else
        {
            return a % b;
        }
    }

    // Calculates Local Sidereal Time at given longitude and date / time
    // LST is a measurment of the angle along the celestial equator from the observer's meridian to the celestial meridian, or 0h RA
    public static float LocalSiderealTime(float longitude, System.DateTime dt)
    {
        // Algorithm taken from Astronomical Algorithms by Jean Meeus

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

    // Calculates the Julian Day of the given Gregorian Date, not including fractional days due to time of day
    // Will return a JD ending in 0.5, as JD's start at 12:00 UTC
    public static double JulianDay(System.DateTime dt)
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

        double julian_day = c + day + e + f - 1524.5f;

        return julian_day;
    }

    public static Vector3 SolarCoordinates(System.DateTime dt)
    {
        //dt = dt.ToUniversalTime();

        // Julian Day calculations
        double current_julian_day = JulianDay(dt);
        double hour_fraction = dt.Hour / 24.0 + dt.Minute / 1440.0 + dt.Second / 86400.0;
        double adjusted_jd = current_julian_day + hour_fraction;
        double time_passed = (adjusted_jd - JULIAN_EPOCH) / 36525.0d;

        // Mean Longitude of the Sun
        double mean_longitude = 280.46646 +
                                36000.76983 * time_passed -
                                0.0001537 * System.Math.Pow(time_passed, 2);
        double mean_longitude_degrees = AngleMod(mean_longitude, 360.0d);
        double mean_longitude_radians = mean_longitude_degrees * System.Math.PI / 180.0d;

        // Mean anomaly of the Sun
        double mean_anomaly = 357.52911 +
                              35999.05029 * time_passed -
                              0.0001537 * System.Math.Pow(time_passed, 2);
        double mean_anomaly_degrees = AngleMod(mean_anomaly, 360.0d);
        double mean_anomaly_radians = mean_anomaly_degrees * System.Math.PI / 180.0d;

        double earth_eccentricity = 0.016708634 -
                                    0.000042037 * time_passed -
                                    0.0000001267 * System.Math.Pow(time_passed, 2);

        double sun_center_degrees = (1.914602 - 0.004817 * time_passed - 0.000014 * System.Math.Pow(time_passed, 2)) * System.Math.Sin(mean_anomaly_radians) +
                            (0.019993 - 0.000101 * time_passed) * System.Math.Sin(2 * mean_anomaly_radians) +
                            (0.000289 * System.Math.Sin(3 * mean_anomaly_radians));

        // Corrections to longitude and anomaly values
        double true_longitude_degrees = mean_longitude_degrees + sun_center_degrees;

        // Apparent longitude corrections
        double omega = 125.04 -
                       1934.136 * time_passed;
        double lambda = true_longitude_degrees -
                        0.00569 -
                        0.00478 * System.Math.Sin(omega * System.Math.PI / 180.0d);

        // Obliquity of the Ecliptic
        double obliquity_of_ecliptic_degrees = 23.439291 -
                                               0.0116153 * time_passed -
                                               1.64e-7 * System.Math.Pow(time_passed, 2) +
                                               5.0361e-7 * System.Math.Pow(time_passed, 3);

        // Obliquity of the Ecliptic corrections for apparent position of the Sun
        double obliquity_degrees_corrected = obliquity_of_ecliptic_degrees +
                                             0.00256 * System.Math.Cos(omega * System.Math.PI / 180.0d);
        double obliquity_radians_corrected = obliquity_degrees_corrected * System.Math.PI / 180.0d;

        // Celestial coordinate calculations
        double right_ascension = System.Math.Atan2(System.Math.Cos(obliquity_radians_corrected) *
                                                   System.Math.Sin(lambda * System.Math.PI / 180.0d) ,
                                                   System.Math.Cos(lambda * System.Math.PI / 180.0d));
        double right_ascension_degrees = AngleMod(right_ascension / System.Math.PI * 180.0d, 360.0d);
        double right_ascension_radians = right_ascension_degrees * System.Math.PI / 180.0d;

        double declination = System.Math.Asin(System.Math.Sin(obliquity_radians_corrected) *
                                              System.Math.Sin(lambda * System.Math.PI / 180.0d));
        double declination_degrees = declination / System.Math.PI * 180.0d; // Only used for debugging and test case checking
        double declination_radians = declination;

        // Convert RA / Dec to Unity Vector3
        Vector3 solar_coordinates = CoordConversion((float)right_ascension_radians, (float)declination_radians);

        return solar_coordinates;
    }
}