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
        SceneManager.LoadScene("TestLaboratory");
        Debug.Log("Return to test laboratory...");
    }

    public void ExitScene()
    {
        Debug.Log("Exit scene...");
        Application.Quit();
    }
    
    public static void ToBondScene()
    {
        SceneManager.LoadScene("TestSharon");
        Debug.Log("Go to bond scene...");
    }
}
