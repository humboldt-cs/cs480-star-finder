using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro3 : MonoBehaviour
{
    public UnityEngine.Gyroscope gyro;
    //GameObject horizon;
 
    // Start is called before the first frame update
    void Start()
    {
        ///horizon = GameObject.Find("Horizon");
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
        }
    }

    private Quaternion GyroToUnity(Quaternion q) 
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
