using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateGPSText : MonoBehaviour
{
    Viewer viewer;
    Text gps_lat;
    Text gps_long;
    Text local_time;
    private void Start()
    {
        viewer = GameObject.Find("Viewer").GetComponent<Viewer>();
        gps_lat = GameObject.Find("UI/Latitude").GetComponent<Text>();
        gps_long = GameObject.Find("UI/Longitude").GetComponent<Text>();
        local_time = GameObject.Find("UI/Time").GetComponent<Text>();

        gps_lat.text = "Lat: " + viewer.getLatitude().ToString();
        gps_long.text = "Long: " + viewer.getLongitude().ToString();
    }


    // Update is called once per frame
    void Update()
    {
        local_time.text = "Local Time: " + System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
