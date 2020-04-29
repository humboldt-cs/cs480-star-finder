using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float test_latitude = 34.227904f;
        float test_longitude = -116.859673f;

        transform.Rotate(StarMath.getRotation(test_latitude, test_longitude, System.DateTime.Now));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Inputs for changing rotation of the earth on z/c inputs
        // Used for testing coordinate conversions, may be reinstated later
        if(Input.GetKey("z"))
        {
            transform.Rotate(Vector3.up, Space.World);
        }
        if (Input.GetKey("c"))
        {
            transform.Rotate(Vector3.down, Space.World);
        }
        */
    }
}
