using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System.Data.Common;
using System;

public class BigBang: MonoBehaviour
{
    // fields
    private SQLiteHelper sqlhelper;
    [SerializeField]
    private GameObject star_prefab;
    [SerializeField]
    private GameObject sun_prefab;
    [SerializeField]
    private Material constellation_mat;

    // Start is called before the first frame update
    void Start()
    {
        // check persistent data path for database, copy from streaming assets if needed
        DatabasePathChecker.CheckPersistentPath();
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // create night sky objects
        CreateStars();
        CreateSun();
        CreateConstellationSegments();

        
    }

    private void CreateConstellationSegments()
    {
        // query database to get constellation segment endpoints
        string inner_query = "SELECT " + DbNames.CONSTELLATION_SEGMENTS_ID + ", "
                                       + DbNames.STAR_DATA_RA + ", "
                                       + DbNames.STAR_DATA_DEC + ", "
                                       + DbNames.CONSTELLATION_SEGMENTS_STAR2 + " "
                           + "FROM " + DbNames.CONSTELLATION_SEGMENTS + " "
                           + "INNER JOIN " + DbNames.STAR_DATA + " ON "
                                       + DbNames.STAR_DATA + "." + DbNames.STAR_DATA_ID + " = "
                                       + DbNames.CONSTELLATION_SEGMENTS + "." + DbNames.CONSTELLATION_SEGMENTS_STAR1;

        string outer_query = "SELECT a." + DbNames.CONSTELLATION_SEGMENTS_ID + ", "
                                       + "a." + DbNames.STAR_DATA_RA + ", "
                                       + "a." + DbNames.STAR_DATA_DEC + ", "
                                       + DbNames.STAR_DATA + "." + DbNames.STAR_DATA_RA + ", "
                                       + DbNames.STAR_DATA + "." + DbNames.STAR_DATA_DEC + " "
                           + "FROM (" + inner_query + ") AS a "
                                       + "INNER JOIN " + DbNames.STAR_DATA + " ON "
                                       + DbNames.STAR_DATA + "." + DbNames.STAR_DATA_ID + " = "
                                       + "a." + DbNames.CONSTELLATION_SEGMENTS_STAR2;
       
        DbDataReader dbReader = sqlhelper.QueryDB(outer_query);

        // local variables
        string id;
        float ra1;
        float dec1;
        float ra2;
        float dec2;
        Vector3 position1;
        Vector3 position2;
        Vector3 segment_vect;
        Vector3 offset_vect;

        while (dbReader.Read())
        {
            // Convert data types appropriately
            id = dbReader[0].ToString();
            ra1 = System.Convert.ToSingle(dbReader[1]);
            dec1 = System.Convert.ToSingle(dbReader[2]);
            ra2 = System.Convert.ToSingle(dbReader[3]);
            dec2 = System.Convert.ToSingle(dbReader[4]);

            // get vector positions stars (multiply by 1.1 to make sure
            // the lines render behind the stars)
            position1 = StarMath.CoordConversion(ra1, dec1)*1.1f;
            position2 = StarMath.CoordConversion(ra2, dec2)*1.1f;
            
            // calculate and offset from each star
            segment_vect = position2 - position1;
            offset_vect = segment_vect.normalized * 8;
            position1 += offset_vect;
            position2 -= offset_vect;

            // Create a new line and draw it
            GameObject constellation_segment = new GameObject(id);
            constellation_segment.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = constellation_segment.GetComponent<LineRenderer>();

            // Line settings
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, position1);
            lineRenderer.SetPosition(1, position2);
            lineRenderer.startWidth = 3.0f;
            lineRenderer.endWidth = 3.0f;
            lineRenderer.numCapVertices = 11;
            lineRenderer.material = constellation_mat;
        }
        dbReader.Close();
    }

    private void CreateStars()
    {
        // query database for star data
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT " + DbNames.STAR_DATA_ID + ", "
                                                             + DbNames.STAR_DATA_RA + ", "
                                                             + DbNames.STAR_DATA_DEC + ", "
                                                             + DbNames.STAR_DATA_MAG + " " +
                                                   "FROM " + DbNames.STAR_DATA);

        string id;
        float ra;
        float dec;
        float mag;
        Vector3 position;
        Vector3 scale;

        while (dbReader.Read())
        {
            id = dbReader[0].ToString();
            ra = System.Convert.ToSingle(dbReader[1]);
            dec = System.Convert.ToSingle(dbReader[2]);
            mag = System.Convert.ToSingle(dbReader[3]);

            position = StarMath.CoordConversion(ra, dec);
            // some of the positions aren't read correctly, 
            scale = StarMath.ScaleFactor(mag);

            GameObject star = Instantiate(star_prefab, position, Quaternion.identity);

            star.name = id;
            star.transform.localScale = scale;
            star.transform.tag = SelectionManager.SELECTABLE_TAG;
            star.AddComponent<SphereCollider>();
        }
        dbReader.Close();
    }

    private void CreateSun()
    {
        float sun_magnitude = -12.0f;

        Vector3 position = StarMath.SolarCoordinates(System.DateTime.Now);
        Vector3 scale = StarMath.ScaleFactor(sun_magnitude);

        GameObject sun = Instantiate(sun_prefab, position, Quaternion.identity);
        
        sun.name = "Sun";
        sun.transform.LookAt(new Vector3(0, 0, 0));
        sun.transform.localScale = scale;
    }
}
