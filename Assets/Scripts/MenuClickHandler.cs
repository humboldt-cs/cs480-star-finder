using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System.Data.Common;
using UnityEngine.UI;

public class MenuClickHandler : MonoBehaviour
{
    [SerializeField] private GameObject menu_prefab;
    [SerializeField] private Object starFinder_prefab;
    [SerializeField] private Text search_status;
    [SerializeField] private InputField search_field;
    private string search_text;
    private string search_star;
    private SQLiteHelper sqlhelper;

    private void Start()
    {
        sqlhelper = new SQLiteHelper();
    }

    public void toStarView()
    {
        // resume camera movement
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            mainCamera.GetComponent<MainCamera>().resume();
        }

        // Destroy settings menu
        GameObject.Destroy(menu_prefab);
    }

    public void getSearchText(string text)
    {
        search_text = text;
    }

    public void findStar()
    {
        string query_string = "SELECT " + DbNames.STAR_DATA_ID + " " +
                              "FROM " + DbNames.STAR_DATA + " " +
                              "WHERE " + DbNames.STAR_DATA_NAME + " = '" + search_text + "'";

        DbDataReader dbReader = sqlhelper.QueryDB(query_string);
        dbReader.Read();
        search_star = dbReader[0].ToString();

        if (!string.IsNullOrEmpty(search_star))
        {
            starFinder_prefab = Resources.Load("Prefabs/StarFinder");
            GameObject starFinderGameObject = Instantiate(starFinder_prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            starFinderGameObject.GetComponent<StarFinder>().activate("HR" + search_star);
            toStarView();
        }
        else
        {
            search_status.text = "Could not find star '" + search_text + "'";
            search_field.text = "";
        }
        dbReader.Close();
    }
}
