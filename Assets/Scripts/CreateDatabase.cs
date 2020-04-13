using UnityEngine;
using SQLite;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.IO;

public class CreateDatabase : MonoBehaviour
{
    private const string STAR_POSITIONS_DATA_SOURCE = "YBSC";
    private SQLiteHelper sqlhelper;

    void Start()
    {
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // Create and populate database tables
        PopulateStarPositions();

        // testing to see if the table was populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + DbNames.STAR_POSITIONS + " LIMIT 5");
        string name;
        string ra;
        string dec;
        string mag;
        while (dbReader.Read())
        {
            name = dbReader[0].ToString();
            ra = dbReader[1].ToString();
            dec = dbReader[2].ToString();
            mag = dbReader[3].ToString();
        }
    }

    // Creates and populates the star_positions table from binary file
    private void PopulateStarPositions()
    {
        // create star data table in db if not already created
        sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + DbNames.STAR_POSITIONS); // This is for testing, comment out if not testing
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