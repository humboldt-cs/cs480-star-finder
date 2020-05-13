using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        // start async operation
        StartCoroutine(LoadAsyncOperation());
    }

    private IEnumerator LoadAsyncOperation()
    {
        // create async operation
        AsyncOperation mainScene = SceneManager.LoadSceneAsync(1);
        
        // update progress bar with async operation progress
        while (mainScene.progress < 1)
        {
            progressBar.fillAmount = mainScene.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
