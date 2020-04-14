using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(GPSOrientation.getRotation(0, 90, 0), Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
