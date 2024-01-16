using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSubmission : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("GlobalChemistryData.instance.substanceResult = " + GlobalChemistryData.instance.substanceResult);
        Debug.Log("GlobalChemistryData.instance.molesOfSubstanceResult = " + GlobalChemistryData.instance.molesOfSubstanceResult);
        Debug.Log("GlobalChemistryData.instance.mixedChemicalCombined = " + GlobalChemistryData.instance.mixedChemicalCombined);
        Debug.Log("GlobalChemistryData.instance.mixedChemicalCombinedAmount = " + GlobalChemistryData.instance.mixedChemicalCombinedAmount);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Container")
        {
            Debug.Log("Collided with container");
            Debug.Log("GlobalChemistryData.instance.mixedChemicalCombined = " + GlobalChemistryData.instance.mixedChemicalCombined);
            Debug.Log("GlobalChemistryData.instance.mixedChemicalCombinedAmount = " + GlobalChemistryData.instance.mixedChemicalCombinedAmount);
            if (GlobalChemistryData.instance.mixedChemicalCombined == GlobalChemistryData.instance.substanceResult &&
                GlobalChemistryData.instance.mixedChemicalCombinedAmount == GlobalChemistryData.instance.molesOfSubstanceResult)
            {
                GlobalChemistryData.instance.gameStatus = "Finished";
            } else
            {
                GlobalChemistryData.instance.gameStatus = "Incorrect";
            }
            Debug.Log("status " + GlobalChemistryData.instance.gameStatus);
        }
    }
}
