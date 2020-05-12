using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClickHandler : MonoBehaviour
{
    [SerializeField] private GameObject menu_prefab;
    [SerializeField] private Object starFinder_prefab;

    public void toStarView()
    {
        // Destroy settings menu
        GameObject.Destroy(menu_prefab);
    }

    public void findStar()
    {
        starFinder_prefab = Resources.Load("Prefabs/StarFinder");
        GameObject starFinderGameObject = Instantiate(starFinder_prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        starFinderGameObject.GetComponent<StarFinder>().activate("HR2491");
        toStarView();
    }
}
