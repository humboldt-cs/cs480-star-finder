using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System.Data.Common;
using System.Xml;

public class StarDBManager : MonoBehaviour
{
    // ====================================================
    // Database table names and descriptions
    // ====================================================
    // * indicates primary key

    // Table for holding star positions and magnitudes
    public const string STAR_POSITIONS = "star_positions";
    private const string STAR_POSITIONS_DATA_SOURCE = "Assets/Resources/star_positions.xml";
    // Table name:  star_positions
    // Columns:     name*       (TEXT data class)       -The standard name for the star, using the HR prefix.
    //              ra          (REAL data class)       -Right ascension in degrees
    //              dec         (REAL data class)       -Declination in degrees
    //              vmag        (REAL data class)       -Photographic magnitude of the star

    // ====================================================
    // Field definitions
    // ====================================================

    private SQLiteHelper sqlhelper;

    // ====================================================
    // Methods
    // ====================================================

    // Start is called before the first frame update
    void Start()
    {
        // create SQL helper object (opens connection to database, handles db interaction)
        sqlhelper = new SQLiteHelper();

        // Create and populate database tables
        StarPositionsTable();

        // testing to see if the table was populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + STAR_POSITIONS + " LIMIT 5");
        string name;
        string ra;
        string dec;
        string vmag;
        while (dbReader.Read())
        {
            name = dbReader[0].ToString();
            ra = dbReader[1].ToString();
            dec = dbReader[2].ToString();
            vmag = dbReader[3].ToString();
        }
        
    }

    // Creates and populates the star_positions table
    private void StarPositionsTable()
    {
        // create star data table in db if not already created
        // sqlhelper.ModifyDB("DROP TABLE IF EXISTS " + STAR_POSITIONS); // This is for testing, comment out if not testing
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS " + STAR_POSITIONS
                           + " (name TEXT PRIMARY KEY, "
                           + "ra REAL, "
                           + "dec REAL, "
                           + "vmag REAL)");
        // check to see if table is already populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM " + STAR_POSITIONS);
        // if the table is empty, read rows from xml file
        if (!dbReader.HasRows)
        {
            dbReader.Close();
            // open xml file and read rows into database
            XmlTextReader xmlReader = new XmlTextReader(STAR_POSITIONS_DATA_SOURCE);
            List<string> xmlDataRow;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "TR")
                    {
                        xmlDataRow = xmlReadRow(xmlReader);
                        string insert_row_statement = "INSERT INTO " + STAR_POSITIONS
                                                      + " VALUES ('"
                                                      + xmlDataRow[1] + "',"
                                                      + xmlDataRow[2] + ","
                                                      + xmlDataRow[3] + ","
                                                      + xmlDataRow[4] + ")";
                        try
                        {
                            sqlhelper.ModifyDB(insert_row_statement);
                        }
                        catch
                        {
                            Debug.Log("skipped row in " + STAR_POSITIONS_DATA_SOURCE + ": "+ xmlDataRow[0]);
                            continue;
                        }

                    }
                }
            }
        }
        dbReader.Close();
    }

    // healper for extracting data from star_positions.xml
    private List<string> xmlReadRow(XmlTextReader xmlReader)
    {
        List<string> data_row = new List<string> { };
        bool end_row = false;
        bool element_open = false;
        while (end_row == false)
        {
            xmlReader.Read();
            switch (xmlReader.NodeType)
            {
                case XmlNodeType.Element:
                    element_open = true;
                    break;
                case XmlNodeType.Text:
                    data_row.Add(xmlReader.Value);
                    element_open = false;
                    break;
                case XmlNodeType.EndElement:
                    if (element_open)
                    {
                        data_row.Add("");
                    }
                    else if (xmlReader.Name == "TR")
                    {
                        end_row = true;
                    }
                    element_open = false;
                    break;
            }
        }
        return data_row;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
