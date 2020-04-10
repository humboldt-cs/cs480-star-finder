using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawConstellation : MonoBehaviour
{
    private const string CONSTELLATION_NODES_FILE = "constellation_star_nodes";
    private const string CONSTELLATION_LINES_FILE = "constellation_lines";

    public LineRenderer constellation_line;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset nodes_lines = Resources.Load<TextAsset>(CONSTELLATION_NODES_FILE);
        TextAsset lines_lines= Resources.Load<TextAsset>(CONSTELLATION_LINES_FILE);

        string[] nodes = nodes_lines.text.Split('\n');
        string[] lines = lines_lines.text.Split('\n');

        // Loop through constellation lines and set line vertices 
        for(int i = 1; i < lines.Length - 1; i++)
        {
            // String formatting
            lines[i] = lines[i].Trim('\r');
            string[] current_line = lines[i].Split(',');
            string[] star_sequence = current_line[1].Split('-');

            // Set number of nodes
            constellation_line.positionCount = star_sequence.Length;

            // Loop through nodes and find positions
            for(int j = 0; j < star_sequence.Length; j++)
            {
                // If value is valid, find star, find position, set vertex
                string[] star = FindStar(nodes, star_sequence[j]);
                Vector3 node_position = GetVertexPosition(star);
                constellation_line.SetPosition(j, node_position);
            }

            // Draw line after all vertices positions are set
            Instantiate(constellation_line);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Finds and returns star data string for star with given unique star_id
    string[] FindStar(string[] stars, string star_id)
    {
        // Loop through stars and find star with id
        for(int i = 0; i < stars.Length; i++)
        {
            stars[i].Trim('\r');
            string[] current_star = stars[i].Split(',');

            // Star found
            if(current_star[0] == star_id)
            {
                return current_star;
            }
        }

        // Star not found
        return null;
    }

    // Converts star string data into usable transform vector
    Vector3 GetVertexPosition(string[] star)
    {
        Vector3 position;

        float ra_hrs = System.Convert.ToSingle(star[2]);
        float ra_min = System.Convert.ToSingle(star[3]);
        float ra_sec = System.Convert.ToSingle(star[4]);

        char dec_sign = System.Convert.ToChar(star[5]);
        float dec_degree = System.Convert.ToSingle(star[6]);
        float dec_arcmin = System.Convert.ToSingle(star[7]);
        float dec_arcsec = System.Convert.ToSingle(star[8]);

        float ra_rad = StarMath.RightAscensionToRadians(ra_hrs, ra_min, ra_sec);
        float dec_rad = StarMath.DeclinationToRadians(dec_sign, dec_degree, dec_arcmin, dec_arcsec);

        position = StarMath.CoordConversion(ra_rad, dec_rad, 0.0f);

        return position;
    }
}
