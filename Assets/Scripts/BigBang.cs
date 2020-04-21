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

                position = StarMath.CoordConversion(ra, dec, mag);

                GameObject star = Instantiate(star_prefab, position, Quaternion.identity);

                star.name = id;
            }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
