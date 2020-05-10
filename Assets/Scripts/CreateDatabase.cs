using UnityEngine;
using SQLite;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Collections.Generic;

public class CreateDatabase : MonoBehaviour
{
    private const string STAR_DATA_SOURCE = "stardata";
    private const string STAR_NAMES_SOURCE = "starnames";
    private const string CONSTELLATION_DATA_SOURCE = "constellation_data";
    private const string CONSTELLATION_SEGMENTS_DATA_SOURCE = "constellation_segments";
    private const string SPECIAL_CHARS_DATA_SOURCE = "special_chars";
    private SQLiteHelper sqlhelper;

    void Start()
    {
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // Create and populate database tables
        PopulateConstellationData();
        PopulateStarData();
        PopulateConstellationSegments();

        // testing to see if the constellation data table was populated
        DbDataReader dbReader0 = sqlhelper.QueryDB("SELECT * FROM " + DbNames.CONSTELLATION_DATA + " LIMIT 5");
        string con_id;
        string con_name;
        string con_gen;
        while (dbReader0.Read())
        {
            con_id = dbReader0[0].ToString();
            con_name = dbReader0[1].ToString();
            con_gen = dbReader0[2].ToString();
        }
        dbReader0.Close();

        // testing to see if the star data table was populated
        DbDataReader dbReader1 = sqlhelper.QueryDB("SELECT * FROM " + DbNames.STAR_DATA + " LIMIT 20");
        string star_id;
        string name;
        string bayer;
        string exp;
        string flam;
        string con;
        string ra;
        string dec;
        string dist;
        string mag;
        string amag;
        while (dbReader1.Read())
        {
            star_id = dbReader1[0].ToString();
            name = dbReader1[1].ToString();
            bayer = dbReader1[2].ToString();
            exp = dbReader1[3].ToString();
            flam = dbReader1[4].ToString();
            con = dbReader1[5].ToString();
            ra = dbReader1[6].ToString();
            dec = dbReader1[7].ToString();
            dist = dbReader1[8].ToString();
            mag = dbReader1[9].ToString();
            amag = dbReader1[10].ToString();
        }
        dbReader1.Close();

        // testing to see if the constellation segments table was populated
        DbDataReader dbReader2 = sqlhelper.QueryDB("SELECT * FROM " + DbNames.CONSTELLATION_SEGMENTS + " LIMIT 5");
        string segment_id;
        string star1;
        string star2;
        string constellation;
        while (dbReader2.Read())
        {
            segment_id = dbReader2[0].ToString();
            star1 = dbReader2[1].ToString();
            star2 = dbReader2[2].ToString();
            constellation = dbReader2[3].ToString();
        }
        dbReader2.Close();
    }

    private void PopulateConstellationData()
    {
        // create constellation data table in db if not already created
        sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + DbNames.CONSTELLATION_DATA); // This is for testing, comment out if not testing
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS " + DbNames.CONSTELLATION_DATA + " ("
                           + DbNames.CONSTELLATION_DATA_ID + " TEXT PRIMARY KEY, "
                           + DbNames.CONSTELLATION_DATA_NAME + " TEXT, "
                           + DbNames.CONSTELLATION_DATA_GEN + " TEXT)");
        // check to see if table is already populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + DbNames.CONSTELLATION_DATA + " LIMIT 1");
        if (!dbReader.HasRows)
        {
            dbReader.Close();
            // open data file
            TextAsset constellation_data = Resources.Load<TextAsset>(CONSTELLATION_DATA_SOURCE);
            string[] constellations = constellation_data.text.Split('\n');

            // local variables
            string[] con_info;
            string info_string;
            string con_id;
            string con_name;
            string con_gen;
            string insert_statement;

            //loop through data
            foreach (string constellation in constellations)
            {
                info_string = constellation.Trim('\r');
                con_info = info_string.Split(',');

                con_id = con_info[0];
                con_name = con_info[1];
                con_gen = con_info[2];

                insert_statement = "INSERT INTO " + DbNames.CONSTELLATION_DATA
                                                  + " VALUES ('"
                                                  + con_id + "','"
                                                  + con_name + "','"
                                                  + con_gen + "')";

                try
                {
                    sqlhelper.ModifyDB(insert_statement);
                }
                catch
                {
                    Debug.Log("insertion failed: " + con_name);
                    continue;
                }
            }
        }
        dbReader.Close();
    }

    private void PopulateConstellationSegments()
    {
        // create constellation segment data table in db if not already created
        sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + DbNames.CONSTELLATION_SEGMENTS); // This is for testing, comment out if not testing
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS " + DbNames.CONSTELLATION_SEGMENTS + " ("
                           + DbNames.CONSTELLATION_SEGMENTS_ID + " INTEGER PRIMARY KEY, "
                           + DbNames.CONSTELLATION_SEGMENTS_STAR1 + " INTEGER, "
                           + DbNames.CONSTELLATION_SEGMENTS_STAR2 + " INTEGER, "
                           + DbNames.CONSTELLATION_SEGMENTS_CON + " TEXT, "
                           + "FOREIGN KEY (" + DbNames.CONSTELLATION_SEGMENTS_STAR1 + ") "
                           + "REFERENCES " + DbNames.STAR_DATA + " (" + DbNames.STAR_DATA_ID + "), "
                           + "FOREIGN KEY (" + DbNames.CONSTELLATION_SEGMENTS_STAR2 + ") "
                           + "REFERENCES " + DbNames.STAR_DATA + " (" + DbNames.STAR_DATA_ID + "),"
                           + "FOREIGN KEY (" + DbNames.CONSTELLATION_SEGMENTS_CON + ") "
                           + "REFERENCES " + DbNames.CONSTELLATION_DATA + " (" + DbNames.CONSTELLATION_DATA_ID + "))"
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
            string segment_id;
            string star1;
            string star2;
            string constellation;
            string insert_statement;

            // loop through data
            foreach (string segment in segments)
            {
                segment_string = segment.Trim('\r');
                segment_data = segment_string.Split(',');

                segment_id = segment_data[0];
                star1 = segment_data[1];
                star2 = segment_data[2];
                constellation = segment_data[3];


                // insert into database
                insert_statement = "INSERT INTO " + DbNames.CONSTELLATION_SEGMENTS
                                                  + " VALUES ("
                                                  + segment_id + ","
                                                  + star1 + ","
                                                  + star2 + ",'"
                                                  + constellation + "')";

                try
                {
                    sqlhelper.ModifyDB(insert_statement);
                }
                catch
                {
                    Debug.Log("insertion failed: " + segment_id);
                    continue;
                }
            }
        }
        dbReader.Close();
    }

    // Creates and populates the star_positions table from stardata.csv
    private void PopulateStarData()
    {
        // create star data table in db if not already created
        sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + DbNames.STAR_DATA); // This is for testing, comment out if not testing
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS " + DbNames.STAR_DATA + " ("
                           + DbNames.STAR_DATA_ID + " INTEGER PRIMARY KEY, "
                           + DbNames.STAR_DATA_NAME + " TEXT, "
                           + DbNames.STAR_DATA_BAYER + " TEXT, "
                           + DbNames.STAR_DATA_BAYER_EXP + " INTEGER, "
                           + DbNames.STAR_DATA_FLAM + " INTEGER, "
                           + DbNames.STAR_DATA_CON + " TEXT, "
                           + DbNames.STAR_DATA_RA + " REAL, "
                           + DbNames.STAR_DATA_DEC + " REAL, "
                           + DbNames.STAR_DATA_DIST + " REAL, "
                           + DbNames.STAR_DATA_AMAG + " REAL, "
                           + DbNames.STAR_DATA_MAG + " REAL, "
                           + "FOREIGN KEY (" + DbNames.STAR_DATA_CON + ") "
                           + "REFERENCES " + DbNames.CONSTELLATION_DATA + " (" + DbNames.CONSTELLATION_DATA_ID + "))"
                           );
        // check to see if table is already populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + DbNames.STAR_DATA + " LIMIT 1");
        // if the table is empty, read rows from xml file
        if (!dbReader.HasRows)
        {
            dbReader.Close();
            // open star data file
            TextAsset star_data = Resources.Load<TextAsset>(STAR_DATA_SOURCE);
            string[] stars = star_data.text.Split('\n');

            // local variables
            string[] star_info;
            string star_string;
            string id_val = "";
            string id_col = "";
            string[] bayer;
            string bayer_val = "";
            string bayer_col = "";
            string exp_val = "";
            string exp_col = "";
            string flam_val = "";
            string flam_col = "";
            string con_val = "";
            string con_col = "";
            string ra_val = "";
            string ra_col = "";
            string dec_val = "";
            string dec_col = "";
            string dist_val = "";
            string dist_col = "";
            string mag_val = "";
            string mag_col = "";
            string amag_val = "";
            string amag_col = "";
            string insert_statement;

            foreach (string star in stars)
            {
                star_string = star.Trim('\r');
                star_info = star_string.Split(',');

                // extract star id
                id_val = star_info[0] + ","; id_col = DbNames.STAR_DATA_ID + ",";
                // extract bayer designation
                if (star_info[1] != "") 
                { 
                    bayer = star_info[1].Split('-');
                    bayer_val = "'" + bayer[0] + "',";
                    bayer_col = DbNames.STAR_DATA_BAYER + ",";
                    if (bayer.Length>1) { exp_val = bayer[1] + ","; exp_col = DbNames.STAR_DATA_BAYER_EXP + ","; }
                    else { exp_val = ""; exp_col = ""; }
                }
                else { bayer_val = ""; bayer_col = ""; exp_val = ""; exp_col = ""; }
                // extract flamsteed designation
                if (star_info[2] != "") { flam_val = star_info[2] + ","; flam_col = DbNames.STAR_DATA_FLAM + ","; } else { flam_val = ""; flam_col = ""; }
                // extract constellation classification
                if (star_info[3] != "") { con_val = "'" + star_info[3] + "',"; con_col = DbNames.STAR_DATA_CON + ","; } else { con_val = ""; con_col = ""; }
                // extract right ascension and declination
                ra_val = star_info[4] + ","; ra_col = DbNames.STAR_DATA_RA + ",";
                dec_val = star_info[5] + ","; dec_col = DbNames.STAR_DATA_DEC + ",";
                // extract distance
                if (star_info[6] != "") { dist_val = star_info[6] + ","; dist_col = DbNames.STAR_DATA_DIST + ","; } else { dist_val = ""; dist_col = ""; }
                // extract absolute magnitude and apparent magnitude
                if (star_info[7] != "") { amag_val = star_info[7] + ","; amag_col = DbNames.STAR_DATA_AMAG + ","; } else { amag_val = ""; amag_col = ""; }
                mag_val = star_info[8]; mag_col = DbNames.STAR_DATA_MAG;

                insert_statement = String.Format("INSERT INTO " + DbNames.STAR_DATA
                                                                + " ({0}{1}{2}{3}{4}{5}{6}{7}{8}{9})" +
                                                 " VALUES ({10}{11}{12}{13}{14}{15}{16}{17}{18}{19})",
                                                 id_col, bayer_col, exp_col, flam_col, con_col, ra_col, dec_col, dist_col, amag_col, mag_col,
                                                 id_val, bayer_val, exp_val, flam_val, con_val, ra_val, dec_val, dist_val, amag_val, mag_val);
                
                try
                {
                    sqlhelper.ModifyDB(insert_statement);
                }
                catch
                {
                    Debug.Log("insert failed: " + id_val);
                    continue;
                }
            }

            // now open star names file
            TextAsset star_names_asset = Resources.Load<TextAsset>(STAR_NAMES_SOURCE);
            string[] star_names = star_names_asset.text.Split('\n');

            // local variables
            string[] row;
            string row_string;
            string id;
            string name;
            string update_statement;

            foreach (string star_name in star_names)
            {
                row_string = star_name.Trim('\r');
                row = row_string.Split(',');
                id = row[0];
                name = row[1];

                update_statement = "UPDATE " + DbNames.STAR_DATA + " " +
                                   "SET " + DbNames.STAR_DATA_NAME + " = '" + name + "' " +
                                   "WHERE " + DbNames.STAR_DATA_ID + " = " + id;

                try
                {
                    sqlhelper.ModifyDB(update_statement);
                }
                catch
                {
                    Debug.Log("update failed: " + id);
                    continue;
                }
            }
        }
        dbReader.Close();
    }
}