using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite
{
    // This class holds the database table names
    public class DbNames
    {
        // Name of the database
        public const string DATABASE_NAME = "Star_finder_Database";

        // Table for holding star positions and magnitudes
        public const string STAR_POSITIONS = "star_positions";  // table name
        public const string STAR_POSITIONS_ID = "id";           // INTEGER type, primary key, Yale Bright Star Catalog id
        public const string STAR_POSITIONS_RA = "ra";           // REAL type, right ascension
        public const string STAR_POSITIONS_DEC = "dec";         // REAL type, declination
        public const string STAR_POSITIONS_MAG = "mag";         // REAL type, photographic magnitude of the star
    }
}
