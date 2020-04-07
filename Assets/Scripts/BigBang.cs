using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System.Data.Common;

public class BigBang: MonoBehaviour
{
    // fields
    private SQLiteHelper sqlhelper;
    [SerializeField]
    private GameObject star_prefab;

    // Start is called before the first frame update
    void Start()
    {
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // query database to get star positions
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + DbNames.STAR_POSITIONS);
        string id;
        float ra;
        float dec;
        float mag;
        Vector3 position;
        
        while (dbReader.Read())
        {
            id = dbReader[0].ToString();
            ra = System.Convert.ToSingle(dbReader[1]);
            dec = System.Convert.ToSingle(dbReader[2]);
            mag = System.Convert.ToSingle(dbReader[3]);

            position = CoordConversion(ra, dec, mag);

            GameObject star = Instantiate(star_prefab, position, Quaternion.identity);

            star.name = id;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Conversion from celestial RA and DEC values to a usable transform vector
    // Expected RA/Dec values to be in radians
    private Vector3 CoordConversion(float right_ascension, float declination, float apparent_magnitude)
    {
        float distance = Mathf.Pow(2, apparent_magnitude) + 20.0f; // A more accurate model would be 2.5^apparent magnitude, this is a demonstration

        float x, y, z;

        x = Mathf.Cos(right_ascension) * Mathf.Cos(declination) * distance;
        z = Mathf.Sin(right_ascension) * Mathf.Cos(declination) * distance;
        y = Mathf.Sin(declination) * distance;

        return new Vector3(x, y, z);
    }
}
