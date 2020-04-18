using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkModeHandler : MonoBehaviour
{
    public GameObject menu_prefab; //= (GameObject)Resources.Load("prefabs/Menu", typeof(GameObject));
    public void toDarkMode()
    {
        // change menu color
        //menu_prefab.GetComponent<Renderer>().material.SetColor("Green", Color.green);
        menu_prefab.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
