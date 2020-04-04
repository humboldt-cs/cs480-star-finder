using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.Common;
using Mono.Data.Sqlite;

namespace SQLite
{
    // This class allows the user to connect to the Bright Star Database and modify/query it.
    // Be sure to close any database readers returned by the QueryDB method before calling other
    // methods on the same SQLiteHelper object
    public class SQLiteHelper
    {
        // database fields
        private const string DBNAME = "Bright_Star_Database";
        private DbConnection dbConnection;
        private DbCommand dbCommand;

        // Constructor
        public SQLiteHelper()
        {
            // Create / open connection to local bright star database
            dbConnection = new SqliteConnection("URI=file:" + Application.persistentDataPath + "/" + DBNAME);
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
}