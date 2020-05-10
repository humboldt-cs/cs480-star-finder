using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using SQLite;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    private SQLiteHelper sqlhelper;
    public const string SELECTABLE_TAG = "Selectable";
    private Transform newSelection = null;
    private Transform oldSelection = null;
    private float originalBrightness = 0f;
    private List<string> highlightedSegments = new List<string>();
    [SerializeField] private Flare highlight_flare;
    [SerializeField] private Flare star_flare;
    [SerializeField] private Text star_text;
    [SerializeField] private Text constellation_text;
    [SerializeField] private Material default_mat;
    [SerializeField] private Material highlight_mat;

    private void Start()
    {
        sqlhelper = new SQLiteHelper();
    }

    private void Update()
    {
        RaycastHit raycastHit;
        
        // cast a sphere from the center of the screen, check to see if it hits anything
        if (Physics.SphereCast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)), 10f, out raycastHit))
        {
            // get the transform of the first object the sphere hit
            newSelection = raycastHit.transform;
            // check if the object is selectable / already selected
            if (newSelection.CompareTag(SELECTABLE_TAG) && (newSelection != oldSelection))
            {
                Deselect(oldSelection, originalBrightness);
                Select(newSelection);
                oldSelection = newSelection;
            }
            else if (!newSelection.CompareTag(SELECTABLE_TAG))
            {
                Deselect(oldSelection, originalBrightness);
                oldSelection = null;
            }
        }
        else
        {
            Deselect(newSelection, originalBrightness);
            newSelection = null;
            oldSelection = null;
        }
    }

    // "Selects" a star
    private void Select(Transform selection)
    {
        // get necisary components
        string selection_id = selection.name.Substring(2);
        LensFlare lensFlare = selection.GetComponent<LensFlare>();

        // get star selection information from database, set UI text
        string[] selection_info = GetSelectionInfo(selection_id);
        string star_name = selection_info[0];
        string star_bayer = selection_info[1];
        string bayer_exp = selection_info[2];
        string con_abv = selection_info[3];
        string con_name = selection_info[4];
        string con_gen = selection_info[5];
        if (!String.IsNullOrEmpty(star_bayer)) { star_bayer = DbNames.GREEK[star_bayer]; }
        if (!String.IsNullOrEmpty(bayer_exp)) { bayer_exp = DbNames.SUPER[System.Convert.ToInt32(bayer_exp)]; }
        if (String.IsNullOrEmpty(star_name)) 
        {
            star_text.text = star_bayer + bayer_exp + " " + con_gen;
        }
        else
        {
            star_text.text = star_name;
        }
        constellation_text.text = "Star in the " + con_name + " constellation";

        // change lens flare on selected star
        originalBrightness = lensFlare.brightness;
        lensFlare.flare = highlight_flare;
        lensFlare.brightness = Mathf.Sqrt(originalBrightness)*10;

        // Highlight the corresponding constellation and reset old highlight
        ResetConstellation(highlightedSegments);
        HighlightConstellation(selection_info[3]);
    }

    

    // "Deselects" a selected star
    private void Deselect(Transform selection, float originalBrightness)
    {
        if (selection == null) { return; }

        // reset UI text
        star_text.text = "";
        constellation_text.text = "";

        // reset lens flare
        LensFlare lensFlare = selection.GetComponent<LensFlare>();
        lensFlare.brightness = originalBrightness;
        lensFlare.flare = star_flare;
    }

    // returns an array of relevent star information from the database
    private string[] GetSelectionInfo(string selection_id)
    {

        string query_string = "SELECT " + DbNames.STAR_DATA_NAME + ", "
                                        + DbNames.STAR_DATA_BAYER + ", "
                                        + DbNames.STAR_DATA_BAYER_EXP + ", "
                                        + DbNames.STAR_DATA_CON + ", "
                                        + DbNames.CONSTELLATION_DATA_NAME + ", "
                                        + DbNames.CONSTELLATION_DATA_GEN + " " +
                              "FROM " + DbNames.STAR_DATA + " " +
                              "INNER JOIN " + DbNames.CONSTELLATION_DATA + " ON "
                                        + DbNames.CONSTELLATION_DATA + "." + DbNames.CONSTELLATION_DATA_ID + " = "
                                        + DbNames.STAR_DATA + "." + DbNames.STAR_DATA_CON + " " +
                              "WHERE " + DbNames.STAR_DATA_ID + " = " + selection_id;

        DbDataReader dbReader = sqlhelper.QueryDB(query_string);
        dbReader.Read();

        List<string> selection_info = new List<string>();

        for (int i=0; i<dbReader.FieldCount; i++)
        {
            selection_info.Add(System.Convert.ToString(dbReader[i]));
        }

        dbReader.Close();
        return selection_info.ToArray();
    }
    
    private void HighlightConstellation(string con)
    {
        string query_string = "SELECT " + DbNames.CONSTELLATION_SEGMENTS_ID + " " +
                              "FROM " + DbNames.CONSTELLATION_SEGMENTS + " " +
                              "WHERE " + DbNames.CONSTELLATION_SEGMENTS_CON + " = '" + con + "'";

        DbDataReader dbReader = sqlhelper.QueryDB(query_string);

        while (dbReader.Read())
        {
            highlightedSegments.Add(dbReader[0].ToString());
        }

        dbReader.Close();

        GameObject segment;
        Renderer renderer;
        foreach (string segment_id in highlightedSegments)
        {
            segment = GameObject.Find("CON" + segment_id);
            if (segment != null)
            {
                renderer = segment.GetComponent<Renderer>();
                if (renderer != null) { renderer.material = highlight_mat; }
            }
        }
    }


    private void ResetConstellation(List<string> highlightedSegments)
    {
        GameObject segment;
        Renderer renderer;
        foreach (string segment_id in highlightedSegments)
        {
            segment = GameObject.Find("CON" + segment_id);
            if (segment != null)
            {
                renderer = segment.GetComponent<Renderer>();
                if (renderer != null) { renderer.material = default_mat; }
            }
        }

        // clear highlighted segments list
        highlightedSegments.Clear();
    }
}
