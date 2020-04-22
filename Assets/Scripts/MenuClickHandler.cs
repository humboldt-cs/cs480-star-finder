using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClickHandler : MonoBehaviour
{
    public GameObject menu_prefab;

    public void toStarView()
    {
        // Destroy settings menu
        GameObject.Destroy(menu_prefab);
    }

}
