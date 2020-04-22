using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkModeHandler : MonoBehaviour
{
    public GameObject menu_prefab;
    public Toggle DarkModeToggle;
    public GameObject off_toggle;
    public Material MenuGrey;
    public Material DarkBlue;

    private void Start()
    {
        off_toggle.GetComponent<Image>().enabled = false;
    }
    public void toDarkMode()
    {
        
        // change menu color
        if (DarkModeToggle.isOn)
        {
            menu_prefab.GetComponent<Image>().material = DarkBlue;
            off_toggle.GetComponent<Image>().enabled = false;
        }
        else
        {
            off_toggle.GetComponent<Image>().enabled = true;
            menu_prefab.GetComponent<Image>().material = MenuGrey;
        }

    }
}
