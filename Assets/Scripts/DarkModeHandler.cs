using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkModeHandler : MonoBehaviour
{
    public GameObject menu_prefab; //= (GameObject)Resources.Load("prefabs/Menu", typeof(GameObject));
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
        //menu_prefab.GetComponent<Renderer>().material.SetColor("Green", Color.green);
        //menu_prefab.GetComponent<Renderer>().material.color = Color.green;
        //menu_prefab.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
