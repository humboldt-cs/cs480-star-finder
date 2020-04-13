using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateGPSText : MonoBehaviour
{
    Text GPSText;
    private void Start()
    {
        GPSText = GameObject.Find("UI/Text").GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        string newLine = Environment.NewLine;
        GPSText.text = "Lat: " + GPS.Instance.latitude.ToString() + newLine + "Long: " + GPS.Instance.longitude.ToString();
    }
}
