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

    private const int CATALOG_START = 28;

    public GameObject star_prefab;

    // Start is called before the first frame update
    void Start()
    {
        // Metadata: catalog headers
        int[] catalog_headers = getCatalogHeaders(bsc_data);

        // Main loop through catalog
        generateStars(star_prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void generateStars(GameObject star_prefab) {
        for (int i = CATALOG_START; i < bsc_data.Length; i += 32)
        {
            // Grab relavent data from star entry
            float catalog_num = System.BitConverter.ToSingle(bsc_data, i);                                  // Bytes 0-3
            float right_ascension = System.Convert.ToSingle(System.BitConverter.ToDouble(bsc_data, i + 4)); // Bytes 4-11.  Includes double -> float conversion
            float declination = System.Convert.ToSingle(System.BitConverter.ToDouble(bsc_data, i + 12));    // Bytes 12-19. Includes double -> float conversion
            float magnitude = System.BitConverter.ToInt16(bsc_data, i + 22) / 100.0f;                       // Bytes 22-24. Includes conversion to decimal value

            // Convert RA/DEC to XYZ
            Vector3 position = CoordConversion(right_ascension, declination, magnitude);

            GameObject star = Instantiate(star_prefab, position, Quaternion.identity);
            star.name = catalog_num.ToString();
        }
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
    private Vector3 CoordConversion(float right_ascension, float declination, float apparent_magnitude)
    {
        float distance = (apparent_magnitude + 1.47f) * 10.0f; // A more accurate model would be 2.5^apparent magnitude, this is a demonstration

        float x, y, z;

        x = Mathf.Cos(right_ascension) * Mathf.Cos(declination) * distance;
        z = Mathf.Sin(right_ascension) * Mathf.Cos(declination) * distance;
        y = Mathf.Sin(declination) * distance;

        return new Vector3(x, y, z);
    }
}
