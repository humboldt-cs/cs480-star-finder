using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var orientation = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0.0f);
        transform.Rotate(orientation);
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    vector3 CoordConversion(double right_ascension, double declination)
    {
        const double DISTANCE_MOD = 10;
        vector3 transform;
        double x, y, z;

        x = Mathf.Sin(right_ascension);
        y = Mathf.Cos(right_ascension);
        z = Mathf.Sin(declination);

        transform = new vector3(x, y, z) * DISTANCE_MOD;

        return transform;
    }
}
