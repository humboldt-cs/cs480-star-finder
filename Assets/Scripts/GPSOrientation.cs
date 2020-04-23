using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPSOrientation
{
    // Test values
    private static float latitude = 90.0f;
    private static float longitude = 0.0f;
    private static float time = 12.0f;

    public static Vector3 getRotation(float latitude, float longitude, float time)
    {
        Vector3 rotation = new Vector3(0, 0, 0);

        rotation.z = longitude;
        rotation.x = latitude - 90.0f;

        return rotation;
    }
}
