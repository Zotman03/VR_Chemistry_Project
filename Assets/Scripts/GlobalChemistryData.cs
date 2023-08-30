using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalChemistryData : MonoBehaviour
{
    public static GlobalChemistryData Instance;

    // chemical substances: substanceOne + substanceTwo -> substanceEnd
    public string substanceOne = "N";
    public string substanceTwo = "H";
    public string substanceResult = "NH";

    // minimum moles of each chemical substance for a reaction
    // molesOfSubstanceOne(substanceOne) + molesOfSubstanceTwo(substanceTwo) -> molesOfSubstanceResult(substanceResult)
    public float molesOfSubstanceOne = 2f;
    public float molesOfSubstanceTwo = 3f;
    public float molesOfSubstanceResult = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
