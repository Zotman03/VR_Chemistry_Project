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
        ES3.DeleteFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset scene...");
    }

    public void PrevScene()
    {
        //ES3AutoSaveMgr.Current.Save();
        Debug.Log("Return to test laboratory...");

        //// load flask liquid colors
        //Material flaskOMaterial = ES3.Load<Material>("flaskOColor");
        //Material flaskNMaterial = ES3.Load<Material>("flaskNColor");
        //Material flaskHMaterial = ES3.Load<Material>("flaskHColor");

        //if (flaskOMaterial != null && flaskNMaterial != null && flaskHMaterial != null)
        //{
        //    Liquid flaskOLiquid = GameObject.Find("Flask O/O SM_Flask_500ml").GetComponentInChildren<Liquid>();
        //    Liquid flaskNLiquid = GameObject.Find("Flask N/N SM_Flask_500ml").GetComponentInChildren<Liquid>();
        //    Liquid flaskHLiquid = GameObject.Find("Flask H/H SM_Flask_500ml").GetComponentInChildren<Liquid>();

        //    if (flaskOLiquid != null && flaskNLiquid != null && flaskHLiquid != null)
        //    {
        //        flaskOLiquid.GetComponent<Renderer>().material = flaskOMaterial;
        //        flaskNLiquid.GetComponent<Renderer>().material = flaskNMaterial;
        //        flaskHLiquid.GetComponent<Renderer>().material = flaskHMaterial;
        //    }
        //}

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
        //// save flask liquid colors
        //GameObject flaskO = GameObject.Find("Flask O/O SM_Flask_500ml");
        //GameObject flaskN = GameObject.Find("Flask N/N SM_Flask_500ml");
        //GameObject flaskH = GameObject.Find("Flask H/H SM_Flask_500ml");

        //if (flaskO != null && flaskN != null && flaskH != null)
        //{
        //    Liquid flaskOLiquid = flaskO.GetComponentInChildren<Liquid>();
        //    Liquid flaskNLiquid = flaskN.GetComponentInChildren<Liquid>();
        //    Liquid flaskHLiquid = flaskH.GetComponentInChildren<Liquid>();

        //    if (flaskOLiquid != null && flaskNLiquid != null && flaskHLiquid != null)
        //    {
        //        ES3.Save<Material>("flaskOColor", flaskOLiquid.GetComponent<Renderer>().material.);
        //        //ES3.Save<Shader>("flaskOColorFoamColor", flaskOLiquid.GetComponent<Renderer>().material.shader);
        //        ES3.Save<Material>("flaskNColor", flaskNLiquid.GetComponent<Renderer>().material);
        //        //ES3.Save<Shader>("flaskNColorFoamColor", flaskNLiquid.GetComponent<Renderer>().material.shader);
        //        ES3.Save<Material>("flaskHColor", flaskHLiquid.GetComponent<Renderer>().material);
        //        //ES3.Save<Shader>("flaskHColorFoamColor", flaskHLiquid.GetComponent<Renderer>().material.shader);
        //    } else
        //    {
        //        Debug.Log("Can't find liquids");
        //    }
        //}
        //else
        //{
        //    Debug.Log("Can't find flasks");
        //}

        //ES3AutoSaveMgr.Current.Save();
        ES3AutoSaveMgr.Current.Save();
        currentScene = SceneManager.GetActiveScene().name;
        ES3.Save("savedLabScene", currentScene);

        SceneManager.LoadScene("BondScene");
        Debug.Log("Go to bond scene...");
    }
}
