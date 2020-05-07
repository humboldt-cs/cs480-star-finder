using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion myRotation;

    // Start is called before the first frame update
    void Start()
    {
        // cameraContainer initialized, set to match camera's movements
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        ///transform.SetParent(cameraContainer.transform);
            
        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro() 
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            //if gyro enabled, set cameraContainer's rotation
            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            myRotation = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * myRotation;
        }
    }
}
