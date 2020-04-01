using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Change orientation of the camera based on arrow key input
        var orientation = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0.0f);
        transform.Rotate(orientation);

        // Change FOV with temporary keys q/e
        Camera.main.fieldOfView = Zoom(Camera.main.fieldOfView);
    }

    float Zoom(float fov)
    {
        // FOV bounds
        const int FOV_MAX = 60;
        const int FOV_MIN = 0;

        // Zoom out
        if (Input.GetKey("q") && fov < FOV_MAX) {
            fov += 1;
        }
        // Zoom in
        if (Input.GetKey("e") && fov > FOV_MIN) {
            fov -= 1;
        }

        return fov;
    }
}
