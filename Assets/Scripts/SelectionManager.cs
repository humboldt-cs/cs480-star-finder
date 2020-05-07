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
    [SerializeField] private Flare highlight_flare;
    [SerializeField] private Flare star_flare;
    [SerializeField] private Text star_text;
    [SerializeField] private Text constellation_text;

    private void Start()
    {
        sqlhelper = new SQLiteHelper();
    }

    private void Update()
    {
        RaycastHit raycastHit;
        
        // cast a sphere from the center of the screen, check to see if it hits anything
        if (Physics.SphereCast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)), 8f, out raycastHit))
        {
            // get the transform of the first object the sphere hit
            newSelection = raycastHit.transform;
            // check if the object is selectable / already selected
            if (newSelection.CompareTag(SELECTABLE_TAG) && (newSelection != oldSelection))
            {
                Deselect(oldSelection, originalBrightness);
                originalBrightness = Select(newSelection);
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
    private float Select(Transform selection)
    {
        if (selection == null) { return 0f; }

        // get necisary components
        string selection_id = selection.name;
        LensFlare lensFlare = selection.GetComponent<LensFlare>();

        // get star selection information from database, set UI text
        string[] selection_info = GetSelectionInfo(selection_id);
        if (String.IsNullOrEmpty(selection_info[0]))
        {
            star_text.text = selection_info[1] + " " + selection_info[4];
        }
        else
        {
            star_text.text = selection_info[0];
        }
        constellation_text.text = "Star in " + selection_info[3];

        // change lens flare on selected star
        originalBrightness = lensFlare.brightness;
        lensFlare.flare = highlight_flare;
        lensFlare.brightness = Mathf.Sqrt(originalBrightness)*10;
        return originalBrightness;
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
}
