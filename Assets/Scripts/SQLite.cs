using UnityEngine;
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

        // Table for holding star positions and magnitudes
        public const string STAR_POSITIONS = "star_positions";  // table name
        public const string STAR_POSITIONS_ID = "star_id";      // INTEGER type, primary key, Yale Bright Star Catalog id
        public const string STAR_POSITIONS_RA = "ra";           // REAL type, right ascension
        public const string STAR_POSITIONS_DEC = "dec";         // REAL type, declination
        public const string STAR_POSITIONS_MAG = "mag";         // REAL type, photographic magnitude of the star

        // Table for holding constellation line segments
        public const string CONSTELLATION_SEGMENTS = "constellation_segments";          // table name
        public const string CONSTELLATION_SEGMENTS_ID = "segment_id";                   // INTEGER type, primary key, arbitrary id number
        public const string CONSTELLATION_SEGMENTS_STAR1 = "star1_id";                  // INTEGER type, foreign key referencing star_positions
        public const string CONSTELLATION_SEGMENTS_STAR2 = "star2_id";                  // INTEGER type, foreign key referencing star_positions
        public const string CONSTELLATION_SEGMENTS_CONSTELLATION = "constellation_id";  // Integer type, foreign key referencing constellations
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
            CheckPersistentPath();
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

        private void CheckPersistentPath()
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