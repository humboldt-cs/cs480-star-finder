using UnityEngine;
using SQLite;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Collections.Generic;

public class CreateDatabase : MonoBehaviour
{
    private const string STAR_POSITIONS_DATA_SOURCE = "YBSC";
    private const string CONSTELLATION_SEGMENTS_DATA_SOURCE = "constellation_segments";
    private SQLiteHelper sqlhelper;

    void Start()
    {
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // Create and populate database tables
        PopulateStarPositions();
        PopulateConstellationSegments();

        // testing to see if the star positions table was populated
        DbDataReader dbReader1 = sqlhelper.QueryDB("SELECT * FROM " + DbNames.STAR_POSITIONS + " ORDER BY " + DbNames.STAR_POSITIONS_MAG + " LIMIT 5");
        string name;
        string ra;
        string dec;
        string mag;
        while (dbReader1.Read())
        {
            name = dbReader1[0].ToString();
            ra = dbReader1[1].ToString();
            dec = dbReader1[2].ToString();
            mag = dbReader1[3].ToString();
        }
        dbReader1.Close();


        // testing to see if the constellation segments table was populated
        DbDataReader dbReader2 = sqlhelper.QueryDB("select * from " + DbNames.CONSTELLATION_SEGMENTS + " LIMIT 5");
        string id;
        string star1;
        string star2;
        string constellation;
        while (dbReader2.Read())
        {
            id = dbReader2[0].ToString();
            star1 = dbReader2[1].ToString();
            star2 = dbReader2[2].ToString();
            constellation = dbReader2[3].ToString();
        }
    }

    private void PopulateConstellationSegments()
    {
        // create star data table in db if not already created
        sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + DbNames.CONSTELLATION_SEGMENTS); // This is for testing, comment out if not testing
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS " + DbNames.CONSTELLATION_SEGMENTS + " ("
                           + DbNames.CONSTELLATION_SEGMENTS_ID + " INTEGER PRIMARY KEY, "
                           + DbNames.CONSTELLATION_SEGMENTS_STAR1 + " INTEGER, "
                           + DbNames.CONSTELLATION_SEGMENTS_STAR2 + " INTEGER, "
                           + DbNames.CONSTELLATION_SEGMENTS_CONSTELLATION + " INTEGER, "
                           + "FOREIGN KEY (" + DbNames.CONSTELLATION_SEGMENTS_STAR1 + ") "
                           + "REFERENCES " + DbNames.STAR_POSITIONS + " (" + DbNames.STAR_POSITIONS_ID + "), "
                           + "FOREIGN KEY (" + DbNames.CONSTELLATION_SEGMENTS_STAR2 + ") "
                           + "REFERENCES " + DbNames.STAR_POSITIONS + " (" + DbNames.STAR_POSITIONS_ID + "))"
                           );
        // check to see if table is already populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + DbNames.CONSTELLATION_SEGMENTS + " LIMIT 1");
        // if the table is empty, read rows from xml file
        if (!dbReader.HasRows)
        {
            dbReader.Close();
            // open data file
            TextAsset constellation_segments = Resources.Load<TextAsset>(CONSTELLATION_SEGMENTS_DATA_SOURCE);
            string[] segments = constellation_segments.text.Split('\n');

            // local variables
            string[] segment_data;
            string segment_string;
            int segment_id;
            int star1;
            int star2;
            int constellation;

            // loop through data
            foreach (string segment in segments)
            {
                segment_string = segment.Trim('\r');
                segment_data = segment_string.Split(',');

                segment_id = Convert.ToInt16(segment_data[0]);
                star1 = Convert.ToInt16(segment_data[1]);
                star2 = Convert.ToInt16(segment_data[2]);
                constellation = Convert.ToInt16(segment_data[3]);


                // insert into database
                string insert_row_statement = "INSERT INTO " + DbNames.CONSTELLATION_SEGMENTS
                                                      + " VALUES ("
                                                      + segment_id + ","
                                                      + star1 + ","
                                                      + star2 + ","
                                                      + constellation + ")";

                try
                {
                    sqlhelper.ModifyDB(insert_row_statement);
                }
                catch
                {
                    Debug.Log("skipped row:" + segment_id);
                    continue;
                }
            }
        }
        dbReader.Close();
    }

    // Creates and populates the star_positions table from binary file
    private void PopulateStarPositions()
    {
        // create star data table in db if not already created
        //sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + DbNames.STAR_POSITIONS); // This is for testing, comment out if not testing
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS " + DbNames.STAR_POSITIONS + " ("
                           + DbNames.STAR_POSITIONS_ID + " INTEGER PRIMARY KEY, "
                           + DbNames.STAR_POSITIONS_RA + " REAL, "
                           + DbNames.STAR_POSITIONS_DEC + " REAL, "
                           + DbNames.STAR_POSITIONS_MAG + " REAL)");
        // check to see if table is already populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + DbNames.STAR_POSITIONS + " LIMIT 1");
        // if the table is empty, read rows from xml file
        if (!dbReader.HasRows)
        {
            dbReader.Close();
            // open data file
            TextAsset catalog = Resources.Load<TextAsset>(STAR_POSITIONS_DATA_SOURCE);
            byte[] bsc_data = catalog.bytes;

            // loop through data
            for (int i = 28; i < bsc_data.Length; i += 32)
            {
                // Grab relavent data from star entry
                float catalog_num = System.BitConverter.ToSingle(bsc_data, i);                                  // Bytes 0-3
                float right_ascension = System.Convert.ToSingle(System.BitConverter.ToDouble(bsc_data, i + 4)); // Bytes 4-11.  Includes double -> float conversion
                float declination = System.Convert.ToSingle(System.BitConverter.ToDouble(bsc_data, i + 12));    // Bytes 12-19. Includes double -> float conversion
                float magnitude = System.BitConverter.ToInt16(bsc_data, i + 22) / 100.0f;                       // Bytes 22-24. Includes conversion to decimal value

                // some stars don't have ra and dec values, so we will ignore those
                if (right_ascension == 0f && declination == 0f) { continue; }

                // insert into database
                string insert_row_statement = "INSERT INTO " + DbNames.STAR_POSITIONS
                                                      + " VALUES ("
                                                      + catalog_num + ","
                                                      + right_ascension + ","
                                                      + declination + ","
                                                      + magnitude + ")";

                try
                {
                    sqlhelper.ModifyDB(insert_row_statement);
                }
                catch
                {
                    Debug.Log("skipped row:" + catalog_num);
                    continue;
                }
            }
        }
        dbReader.Close();
    }
}