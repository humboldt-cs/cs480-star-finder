using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBSC : MonoBehaviour
{
    // Star data provided by Yale Bright Star Catalog
    // URL: http://tdc-www.harvard.edu/catalogs/bsc5.html
    // BSC5 uses custom binary format. Specs: http://tdc-www.harvard.edu/catalogs/catalogsb.html 

    private static string bsc_path = "Assets/Resources/YBSC";
    private static byte[] bsc_data = System.IO.File.ReadAllBytes(bsc_path);

    // Start is called before the first frame update
    void Start()
    {

        int temp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
    Vector3 CoordConversion(float right_ascension, float declination, float apparent_magnitude)
    {
        const float DISTANCE_MIN = 10.0f;
        float distance = (apparent_magnitude + 1) * DISTANCE_MIN; // A more accurate model would be 2.5^apparent magnitude, this is a demonstration

        float x, y, z;

        x = Mathf.Cos(right_ascension) * Mathf.Cos(declination) * distance;
        z = Mathf.Sin(right_ascension) * Mathf.Cos(declination) * distance;
        y = Mathf.Sin(declination) * distance;

        return new Vector3(x, y, z);
    }

    // Grabs the first 28 bytes of catalog data.
    // Value specifications are given here: http://tdc-www.harvard.edu/catalogs/catalogsb.html
    private int[] getCatalogHeaders(byte[] catalog_data) {
        byte[] headers = new byte[28];

        // Grab raw bytes
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = bsc_data[i];
        }

        // Convert 28 bytes to seven 32-bit ints
        int[] header_values = new int[7];
        for (int i = 0; i < headers.Length; i += 4)
        {
            header_values[i / 4] = System.BitConverter.ToInt32(headers, i);
        }

        return header_values;
    }
}
