using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset scene...");
    }

    public void PrevScene()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("TestLaboratory").buildIndex);
        Debug.Log("Return to test laboratory...");
    }

    public void ExitScene()
    {
        Application.Quit();
        Debug.Log("Exit scene...");
    }
    
    public static void ToBondScene()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("TestSharon").buildIndex);
        Debug.Log("Go to bond scene...");
    }
}
