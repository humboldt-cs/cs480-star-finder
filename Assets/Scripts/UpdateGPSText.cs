using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateGPSText : MonoBehaviour
{
    Text gps_lat;
    Text gps_long;
    private void Start()
    {
        gps_lat = GameObject.Find("UI/Latitude").GetComponent<Text>();
        gps_long = GameObject.Find("UI/Longitude").GetComponent<Text>();

    }


    // Update is called once per frame
    void Update()
    {
        string newLine = Environment.NewLine;
        gps_lat.text = "Lat: " + GPS.Instance.latitude.ToString();
        gps_long.text = "Long: " + GPS.Instance.longitude.ToString();
    }
}
