using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = StarMath.SolarCoordinates(System.DateTime.Now);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
