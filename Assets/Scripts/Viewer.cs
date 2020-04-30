using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    private float latitude = 34.227904f;
    private float longitude = -116.859673f;
    private Vector3 second_rotation = new Vector3(0.0f, 0.004166667f, 0.0f);
    private System.DateTime dt;

    // Start is called before the first frame update
    void Start()
    {
        dt = System.DateTime.Now;
        transform.Rotate(StarMath.getRotation(latitude, longitude, dt));
        InvokeRepeating("FollowSky", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Inputs for changing rotation of the earth on z/c inputs
        // Used for testing coordinate conversions, may be reinstated later
        if (Input.GetKey("z"))
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
}
