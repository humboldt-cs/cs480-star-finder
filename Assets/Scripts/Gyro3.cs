using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro3 : MonoBehaviour
{
    public UnityEngine.Gyroscope gyro;
    //Quaternion initialRotation;
    private Quaternion gyroInitialRotation;
    private Quaternion correction;
    public GameObject viewer;
    private bool isSet;

    //GameObject horizon;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            Input.gyro.enabled = true;
        }

        //isSet = false;
        //correction = Quaternion.Inverse(Quaternion.Euler(0f, 0f, 90f));
        //initialRotation = transform.rotation;
        //gyroInitialRotation = GyroToUnity(Input.gyro.attitude);
        //gyroInitialRotation = viewer.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isSet) 
        {
            correction = viewer.transform.rotation * Quaternion.Inverse(GyroToUnity(Input.gyro.attitude));
            isSet = true;
        }*/

        gyroInitialRotation = viewer.transform.rotation;

        if (SystemInfo.supportsGyroscope)
        {
            //transform.rotation = GyroToUnity(Input.gyro.attitude);
            //transform.RotateAround(transform.parent.position, transform.parent.up, Input.GetAxis("Horizontal"));
            //Quaternion offsetRotation = gyroInitialRotation * GyroToUnity(Input.gyro.attitude);
            transform.localRotation = GyroToUnity(Input.gyro.attitude);
        }
    }

    private Quaternion GyroToUnity(Quaternion q) 
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
