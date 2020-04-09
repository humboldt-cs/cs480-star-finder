using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawConstellation : MonoBehaviour
{
    private const string position_resource = "orion";

    public LineRenderer constellation_line;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset star_position_data = Resources.Load<TextAsset>(position_resource);
        string[] stars = star_position_data.text.Split('\n');

        Vector3[] positions = new Vector3[stars.Length - 1];

        // Grab vertex positions
        for(int i = 1; i < stars.Length; i++)
        {
            string[] current_star = stars[i].Split(',');
            positions[i - 1] = GetVertexPosition(current_star);
        }

        LineRenderer line = Instantiate(constellation_line);

        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetVertexPosition(string[] star)
    {
        Vector3 position = Vector3.up;

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
