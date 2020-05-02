using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    // Default values: Greenwich, England UTC
    private float latitude = 51.4934f;
    private float longitude = 0.0098f;
    private System.DateTime dt = System.DateTime.Now.ToUniversalTime();

    private Vector3 second_rotation = new Vector3(0.0f, 0.004166667f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        // Check if location services are enabled
        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS not enabled by user.");
        }
        // Check if location can be found
        else if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Could not find location.");
        }
        // Success, location found and values set to local position / time
        else
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            dt = System.DateTime.Now;
        }

        // Apply rotation according to position / time values
        transform.Rotate(HorizonRotation(latitude, longitude, dt));

        // Follow night sky every second
        InvokeRepeating("FollowSky", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("z"))
        {
            transform.Rotate(Vector3.up, Space.World);
        }
        if (Input.GetKey("c"))
        {
            transform.Rotate(Vector3.down, Space.World);
        }
    }

    void FollowSky()
    {
        transform.Rotate(second_rotation, Space.World);
    }

    Vector3 HorizonRotation(float latitude, float longitude, System.DateTime dt)
    {
        float rotation = StarMath.LocalSiderealTime(longitude, dt);

        return new Vector3(latitude - 90.0f, rotation, 0.0f);
    }

    public float getLatitude()
    {
        return latitude;
    }

    public float getLongitude()
    {
        return longitude;
    }

    public System.DateTime getDateTime()
    {
        return dt;
    }
}
