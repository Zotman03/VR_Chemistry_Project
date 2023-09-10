using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogData : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Top: " + GlobalChemistryData.instance.mixedChemicalOne + " - " + GlobalChemistryData.instance.mixedChemicalOneAmount.ToString() + " moles");
        Debug.Log("Foam: " + GlobalChemistryData.instance.mixedChemicalTwo + " - " + GlobalChemistryData.instance.mixedChemicalTwoAmount.ToString() + " moles");
    }
}
