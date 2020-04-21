using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkModeHandler : MonoBehaviour
{
    public GameObject menu_prefab; 
    public Toggle DarkModeToggle;
    public Material MenuGrey;
    public Material DarkBlue;
    public void toDarkMode()
    {
        // change menu color
        if (DarkModeToggle.isOn)
        {
            menu_prefab.GetComponent<Image>().material = DarkBlue;
        }
        else
        {
            menu_prefab.GetComponent<Image>().material = MenuGrey;
        }

    }
}
