using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public string currentScene;
    public string sceneToLoad;

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset scene...");
    }

    public void PrevScene()
    {
        //ES3AutoSaveMgr.Current.Save();
        Debug.Log("Return to test laboratory...");
        sceneToLoad = ES3.Load<string>("savedLabScene");
        SceneManager.LoadScene(sceneToLoad);
        //SceneManager.LoadScene("Laboratory");
        //ES3AutoSaveMgr.Current.Load();
    }

    public void ExitScene()
    {
        Debug.Log("Exit scene...");
        Application.Quit();
    }

    public void ToBondScene()
    {
        //ES3AutoSaveMgr.Current.Save();
        ES3AutoSaveMgr.Current.Save();
        currentScene = SceneManager.GetActiveScene().name;
        ES3.Save("savedLabScene", currentScene);

        SceneManager.LoadScene("BondScene");
        Debug.Log("Go to bond scene...");
    }
}
