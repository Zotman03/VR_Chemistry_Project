using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSubmission : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("GlobalChemistryData.instance.mixedChemicalCombined = " + GlobalChemistryData.instance.mixedChemicalCombined);
        Debug.Log("GlobalChemistryData.instance.substanceTwo = " + GlobalChemistryData.instance.substanceTwo);
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
            }
            Debug.Log("status " + GlobalChemistryData.instance.gameStatus);
        }
    }
}
