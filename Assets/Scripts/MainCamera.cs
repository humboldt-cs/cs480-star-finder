﻿using System.Collections;
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

            // Rotate camera with single touch
            if (Input.touchCount == 1)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotation_speed = 0.1f; // Speed modifier
                    transform.Rotate(Vector3.right * touch.deltaPosition.y * rotation_speed, Space.Self); // pitch
                    transform.Rotate(Vector3.up * -touch.deltaPosition.x * rotation_speed, Space.World); // yaw
                }
            }

            // Zoom camera with pinch touch
            if(Input.touchCount == 2) {
                // FOV bounds
                const int FOV_MAX = 30;
                const int FOV_MIN = 0;

                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Zoom out
                if(Vector2.Distance(touch1.position, touch2.position) > Vector2.Distance(touch1.position + touch1.deltaPosition, touch2.position + touch2.deltaPosition) 
                    && Camera.main.fieldOfView < FOV_MAX)
                {
                    Camera.main.fieldOfView++;
                }
                // Zoom in
                if(Vector2.Distance(touch1.position, touch2.position) < Vector2.Distance(touch1.position + touch1.deltaPosition, touch2.position + touch2.deltaPosition)
                    && Camera.main.fieldOfView > FOV_MIN)
                {
                    Camera.main.fieldOfView--;
                }
            }
        }

        // Change FOV with keys q/e
        Camera.main.fieldOfView = Zoom(Camera.main.fieldOfView);
    }

    private float Zoom(float fov) {
        // FOV bounds
        const int FOV_MAX = 30;
        const int FOV_MIN = 0;

        // Zoom out
        if (Input.GetKey("q") && fov < FOV_MAX) {
            fov++;
        }
        // Zoom in
        if (Input.GetKey("e") && fov > FOV_MIN) {
            fov--;
        }

        return fov;
    }
}
