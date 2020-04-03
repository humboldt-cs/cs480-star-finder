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
        transform.Rotate(Vector3.right * Input.GetAxis("Vertical"), Space.Self); // pitch
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal"), Space.World); // yaw

        // Change orientation of the camera based on touchscreen input
        // If screen detects touch
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Moved) {
                Vector2 change = touch.deltaPosition;
                float speed = 0.1f;
                transform.Rotate(Vector3.right * change.y * speed, Space.Self); // pitch
                transform.Rotate(Vector3.up * -change.x * speed, Space.World); // yaw
            }
        }

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
