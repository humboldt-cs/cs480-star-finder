using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // Change orientation of the camera based on arrow key input
        float pitch = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.right * pitch, Space.Self);
        transform.Rotate(Vector3.up * yaw, Space.World);

        // Change FOV with temporary keys q/e
        Camera.main.fieldOfView = Zoom(Camera.main.fieldOfView);
    }

    private float Zoom(float fov) {
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
