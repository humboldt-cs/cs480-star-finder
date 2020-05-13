using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBSC : MonoBehaviour
{
    // Star data provided by Yale Bright Star Catalog
    // URL: http://tdc-www.harvard.edu/catalogs/bsc5.html
    // BSC5 uses custom binary format. Specs: http://tdc-www.harvard.edu/catalogs/catalogsb.html

    private const int CATALOG_START = 28;
    private static string catalog_resource = "YBSC";

    public GameObject star_prefab;

    void Awake()
    {

        TextAsset catalog = Resources.Load<TextAsset>(catalog_resource);

        byte[] bsc_data = catalog.bytes;

        // Metadata: catalog headers
        int[] catalog_headers = getCatalogHeaders(bsc_data);

        // Main loop through catalog
        generateStars(bsc_data, star_prefab);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Grabs the first 28 bytes of catalog data.
    // Value specifications are given here: http://tdc-www.harvard.edu/catalogs/catalogsb.html
    private int[] getCatalogHeaders(byte[] catalog_data) {
        byte[] headers = new byte[CATALOG_START];

        // Grab raw bytes
        for (int i = 0; i < CATALOG_START; i++)
        {
            headers[i] = catalog_data[i];
        }

        // Convert 28 bytes to seven 32-bit ints
        int[] header_values = new int[7];
        for (int i = 0; i < headers.Length; i += 4)
        {
            header_values[i / 4] = System.BitConverter.ToInt32(headers, i);
        }

        return header_values;
    }

    private void generateStars(byte[] bsc_data, GameObject star_prefab) {
        for (int i = CATALOG_START; i < bsc_data.Length; i += 32)
        {
            // Grab relavent data from star entry
            float catalog_num = System.BitConverter.ToSingle(bsc_data, i);                                  // Bytes 0-3
            float right_ascension = System.Convert.ToSingle(System.BitConverter.ToDouble(bsc_data, i + 4)); // Bytes 4-11.  Includes double -> float conversion
            float declination = System.Convert.ToSingle(System.BitConverter.ToDouble(bsc_data, i + 12));    // Bytes 12-19. Includes double -> float conversion
            float magnitude = System.BitConverter.ToInt16(bsc_data, i + 22) / 100.0f;                       // Bytes 22-24. Includes conversion to decimal value

            // Convert RA/DEC to XYZ
            Vector3 position = StarMath.CoordConversion(right_ascension, declination, magnitude);

            GameObject star = Instantiate(star_prefab, position, Quaternion.identity);
            star.name = catalog_num.ToString();
        }
    }
}
