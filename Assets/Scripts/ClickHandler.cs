using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{
    public Button menu_button;
    public GameObject menu_prefab;
    [SerializeField] GameObject main_camera;

    public void toMenu()
    {
        // Create menu prefab
        Instantiate(menu_prefab, new Vector3(0, 0, 0), Quaternion.identity);

        // pause camera movement
        main_camera.GetComponent<MainCamera>().pause();
    }
}
