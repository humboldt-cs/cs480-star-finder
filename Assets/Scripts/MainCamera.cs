using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Variable for holding camera FOV
    private float field_of_view;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize FOV to default value
        field_of_view = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        // Change orientation of the camera based on arrow key input
        var orientation = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0.0f);
        transform.Rotate(orientation);

        // Change FOV with Q/E keys
        if(Input.GetKey("q"))
        {
            field_of_view += 1;
        }
        if (Input.GetKey("e"))
        {
            field_of_view -= 1;
        }
        Camera.main.fieldOfView = field_of_view;
    }
}
