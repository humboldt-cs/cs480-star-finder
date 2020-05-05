﻿using UnityEngine;
using System.Data.Common;
using Mono.Data.Sqlite;
using UnityEngine.Networking;
using System.IO;

namespace SQLite
{
    // This class holds the database table names
    public class DbNames
    {
        // Name of the database
        public const string DATABASE_NAME = "Star_finder_Database";

        // Table for holding star data
        public const string STAR_DATA = "star_data";            // table name
        public const string STAR_DATA_ID = "star_id";           // INTEGER type, primary key, Yale Bright Star Catalog id
        public const string STAR_DATA_NAME = "star_name";       // TEXT type, proper name of the star
        public const string STAR_DATA_BAYER = "star_bayer";     // TEXT type, Bayer designation
        public const string STAR_DATA_FLAM = "star_flam";       // INTEGER type, Flamsteed designation
        public const string STAR_DATA_CON = "star_con";         // TEXT type, three character constellation designation
        public const string STAR_DATA_RA = "star_ra";           // REAL type, right ascension
        public const string STAR_DATA_DEC = "star_dec";         // REAL type, declination
        public const string STAR_DATA_DIST = "star_dist";       // REAL type, distance from earth in parsecs
        public const string STAR_DATA_AMAG = "star_amag";       // REAL type, absolute magnitude
        public const string STAR_DATA_MAG = "star_mag";         // REAL type, apparent magnitude

        // Table for holding constellation line segments
        public const string CONSTELLATION_SEGMENTS = "constellation_segments";      // table name
        public const string CONSTELLATION_SEGMENTS_ID = "segment_id";               // INTEGER type, primary key, arbitrary id number
        public const string CONSTELLATION_SEGMENTS_STAR1 = "star1_id";              // INTEGER type, foreign key referencing star_data
        public const string CONSTELLATION_SEGMENTS_STAR2 = "star2_id";              // INTEGER type, foreign key referencing star_data
        public const string CONSTELLATION_SEGMENTS_CON = "con_id";                  // TEXT type, foreign key referencing constellation_data

        // Table for holding constellation data
        public const string CONSTELLATION_DATA = "constellation_data";      // table name
        public const string CONSTELLATION_DATA_ID = "con_id";               // TEXT type, three character abbreviation of constellation name from International Astronomical Union (IAU)
        public const string CONSTELLATION_DATA_NAME = "con_name";           // TEXT type, official name of the constellation
        public const string CONSTELLATION_DATA_GEN = "con_gen";             // TEXT type, genitive version of constellation (follows bayer/flamsteed designation to identify a star)

    }

    // This class is for reading from the database. It reads from the StreamingAssets folder so make
    // sure the database file is in the StreamingAssets folder before using.
    public class SQLiteHelper
    {
        // SQLiteHelper Fields
        private DbConnection dbConnection;
        private DbCommand dbCommand;

        // Constructor
        public SQLiteHelper()
        {
            // Create / open connection to database
            string db_path = "URI=file:" + Application.persistentDataPath + "/" + DbNames.DATABASE_NAME;
            dbConnection = new SqliteConnection(db_path);
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
        }

        // Destructor
        ~SQLiteHelper()
        {
            dbConnection.Close();
        }

        // Method for create, insert, update, delete sql statements
        public int ModifyDB(string sql_non_query_statement)
        {
            dbCommand.CommandText = sql_non_query_statement;
            return dbCommand.ExecuteNonQuery();
        }

        // Method for query sql statements
        public DbDataReader QueryDB(string sql_query_statement)
        {
            dbCommand.CommandText = sql_query_statement;
            return dbCommand.ExecuteReader();
        }
    }

    public static class DatabasePathChecker
    {
        public static void CheckPersistentPath()
        {
            // check if the database file exists on the persistent data path
            string persistent_path = Application.persistentDataPath + "/" + DbNames.DATABASE_NAME;
            bool exists = System.IO.File.Exists(persistent_path);
            // if it does not exist, copy it from the StreamingAssets folder
            string streaming_path = Application.streamingAssetsPath + "/" + DbNames.DATABASE_NAME;
            if (!exists)
            {
                // if the runtime platform is Android or WebGL, we need to use a
                // UnityWebRequest to access the databae file from StreamingAssets
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    UnityWebRequest databse = UnityWebRequest.Get(streaming_path);
                    databse.SendWebRequest();
                    while (!databse.isDone) { }
                    File.WriteAllBytes(persistent_path, databse.downloadHandler.data);
                }
                // otherwise the file can be copied directly
                else
                {
                    File.Copy(streaming_path, persistent_path);
                }
            }
        }
    }
}