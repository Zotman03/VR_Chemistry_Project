using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string mixedChemicalCombined = "TEST"; //"NH2";
    public float mixedChemicalCombinedAmount = 1f; //2f;

    public string gameStatus = "Incorrect";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
