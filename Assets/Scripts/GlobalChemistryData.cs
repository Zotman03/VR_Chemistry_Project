using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalChemistryData : MonoBehaviour
{
    public static GlobalChemistryData instance;

    // chemical substances: substanceOne + substanceTwo -> substanceEnd
    public string substanceOne = "N";
    public string substanceTwo = "H";
    public string substanceResult = "NH2";

    // minimum moles of each chemical substance for a reaction
    // molesOfSubstanceOne(substanceOne) + molesOfSubstanceTwo(substanceTwo) -> molesOfSubstanceResult(substanceResult)
    public float molesOfSubstanceOne = 2f;
    public float molesOfSubstanceTwo = 4f;
    public float molesOfSubstanceResult = 2f;

    public string mixedChemicalOne = "";
    public string mixedChemicalTwo = "";
    public float mixedChemicalOneAmount = 0f;
    public float mixedChemicalTwoAmount = 0f;

    public string mixedChemicalCombined = "";
    public float mixedChemicalCombinedAmount = 0f;

    public string gameStatus = "Incorrect";
    //public bool newGameOLiquid = true;
    //public bool newGameHLiquid = true;
    //public bool newGameNLiquid = true;

    private void Awake()
    {
        if (instance == null)
        {
            ES3.DeleteFile();
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //ES3AutoSaveMgr.Current.Load();
            Destroy(gameObject);
        }
    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log("LOAD SAVE UPON SCENE LOAD" + scene.name);
    //    ES3AutoSaveMgr.Current.Load();
    //}
}
