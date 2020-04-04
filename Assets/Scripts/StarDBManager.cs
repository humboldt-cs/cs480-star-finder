using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System.Data.Common;
using System.Xml;
using System.IO;


public class StarDBManager : MonoBehaviour
{
    private SQLiteHelper sqlhelper;

    // Start is called before the first frame update
    void Start()
    {
        // create SQL helper object (opens connection to database)
        sqlhelper = new SQLiteHelper();
        // create star data table in db if not already created
        sqlhelper.ModifyDB("CREATE TABLE IF NOT EXISTS star_data (id INTEGER PRIMARY KEY, name TEXT, alt_name TEXT, ra REAL, dec REAL, vmag REAL)");
        // check to see if table is already populated
        DbDataReader dbReader = sqlhelper.QueryDB("SELECT * FROM star_data");
        // if the table is empty, read rows from xml file
        if (!dbReader.HasRows)
        {
            dbReader.Close();
            // open xml file and read rows into database
            XmlTextReader xmlReader = new XmlTextReader("Assets/Resources/votable.xml");
            List<string> xmlDataRow;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "TR")
                    {
                        xmlDataRow = xmlReadRow(xmlReader);
                        string insert_row_statement = "INSERT INTO star_data VALUES ("
                                                      + xmlDataRow[0] + ",'"
                                                      + xmlDataRow[1] + "','"
                                                      + xmlDataRow[2] + "',"
                                                      + xmlDataRow[3] + ","
                                                      + xmlDataRow[4] + ","
                                                      + xmlDataRow[5] + ")";
                        try
                        {
                            sqlhelper.ModifyDB(insert_row_statement);
                        }
                        catch
                        {
                            continue;
                        }
                        
                    }
                } 
            }
        }

        dbReader.Close();
        // this is just to test that its working
        DbDataReader dbReader2 = sqlhelper.QueryDB("SELECT * FROM star_data");
        string id;
        string name;
        string alt_name;
        string ra;
        string dec;
        string vmag;
        while (dbReader2.Read())
            {
                id = dbReader2[0].ToString();
                name = dbReader2[1].ToString();
                alt_name = dbReader2[2].ToString();
                ra = dbReader2[3].ToString();
                dec = dbReader2[4].ToString();
                vmag = dbReader2[5].ToString();
            }
    }

    // healper for reading stardata.xml table rows
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
