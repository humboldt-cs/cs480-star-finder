using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using UnityEngine.SceneManagement;

public class AppLoader : MonoBehaviour
{
    private DatabaseLoader databaseLoader;

    // Start is called before the first frame update
    void Start()
    {
        databaseLoader = new DatabaseLoader();
        databaseLoader.LoadData();
        SceneManager.LoadScene("PeterScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
