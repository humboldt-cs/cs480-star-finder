using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroObject : MonoBehaviour
{
    public UnityEngine.Gyroscope gyro;
    GameObject Viewer;

    // Start is called before the first frame update
    void Start()
    {
        Viewer = GameObject.Find("Viewer");
        //transform.rotation = horizon.transform.rotation;

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            Input.gyro.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            transform.rotation = GyroToUnity(Input.gyro.attitude);
            transform.RotateAround(transform.parent.position, transform.parent.up, Input.GetAxis("Horizontal"));
            Viewer.transform.rotation = transform.rotation;
        }
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
