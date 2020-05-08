using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro3 : MonoBehaviour
{
    public UnityEngine.Gyroscope gyro;
    private Quaternion gyroInitialRotation;
    public GameObject GyroObject;
    private bool isSet;

    //GameObject horizon;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            Input.gyro.enabled = true;
            //gyroInitialRotation = GyroToUnity(Input.gyro.attitude);
        }

        //isSet = false;
        gyroInitialRotation = GyroToUnity(Input.gyro.attitude);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isSet) 
        {
            gyroInitialRotation = GyroToUnity(Input.gyro.attitude);
            isSet = true;
        }*/

        if (SystemInfo.supportsGyroscope)
        {
            Quaternion offsetRotation = Quaternion.Inverse(gyroInitialRotation) * GyroToUnity(Input.gyro.attitude);
            transform.rotation = GyroObject.transform.rotation * offsetRotation;
        }
    }

    private Quaternion GyroToUnity(Quaternion q) 
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    void OnGUI()
    {
        if( GUILayout.Button( "Calibrate", GUILayout.Width( 300 ), GUILayout.Height( 100 ) ) )
        {
            CalibrateYAngle();
        }
    }
 
    public void CalibrateYAngle()
    {
        gyroInitialRotation = GyroToUnity(Input.gyro.attitude);
    }

}
