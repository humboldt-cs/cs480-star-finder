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
    private Material constellation_mat;

    // Start is called before the first frame update
    void Start()
    {
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // create night sky objects
        CreateStars();
        CreateConstellationSegments();

        
    }

    private void CreateConstellationSegments()
    {
        // query database to get constellation segment endpoints
        string inner_query = "SELECT " + DbNames.CONSTELLATION_SEGMENTS_ID + ", "
                                       + DbNames.STAR_POSITIONS_RA + ", "
                                       + DbNames.STAR_POSITIONS_DEC + ", "
                                       + DbNames.CONSTELLATION_SEGMENTS_STAR2 + " "
                           + "FROM " + DbNames.CONSTELLATION_SEGMENTS + " "
                           + "INNER JOIN " + DbNames.STAR_POSITIONS + " ON "
                                       + DbNames.STAR_POSITIONS + "." + DbNames.STAR_POSITIONS_ID + " = "
                                       + DbNames.CONSTELLATION_SEGMENTS + "." + DbNames.CONSTELLATION_SEGMENTS_STAR1;

        string outer_query = "SELECT a." + DbNames.CONSTELLATION_SEGMENTS_ID + ", "
                                       + "a." + DbNames.STAR_POSITIONS_RA + ", "
                                       + "a." + DbNames.STAR_POSITIONS_DEC + ", "
                                       + DbNames.STAR_POSITIONS + "." + DbNames.STAR_POSITIONS_RA + ", "
                                       + DbNames.STAR_POSITIONS + "." + DbNames.STAR_POSITIONS_DEC + " "
                           + "FROM (" + inner_query + ") AS a "
                                       + "INNER JOIN " + DbNames.STAR_POSITIONS + " ON "
                                       + DbNames.STAR_POSITIONS + "." + DbNames.STAR_POSITIONS_ID + " = "
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

        while (dbReader.Read())
        {
            // Convert data types appropriately
            id = dbReader[0].ToString();
            ra1 = System.Convert.ToSingle(dbReader[1]);
            dec1 = System.Convert.ToSingle(dbReader[2]);
            ra2 = System.Convert.ToSingle(dbReader[3]);
            dec2 = System.Convert.ToSingle(dbReader[4]);

            // get vector positions of line verticies
            position1 = StarMath.CoordConversion(ra1, dec1, 5.0f);
            position2 = StarMath.CoordConversion(ra2, dec2, 5.0f);

            // Create a new line and draw it
            GameObject constellation_segment = new GameObject(id);
            constellation_segment.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = constellation_segment.GetComponent<LineRenderer>();

            // Line settings
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, position1);
            lineRenderer.SetPosition(1, position2);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = constellation_mat;
        }
    }

    private void CreateStars()
    {
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
        dbReader.Close();
    }
}
