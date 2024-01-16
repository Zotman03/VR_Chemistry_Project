using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabMenuTextValue : MonoBehaviour
{
    [SerializeField]
    TMP_Text myInputField;

    void Update()
    {
        if (GlobalChemistryData.instance.gameStatus == "Finished")
        {
            myInputField.text = "Congrats on completing the lab!";
        }
        else if (GlobalChemistryData.instance.gameStatus == "Incorrect")
        {
            myInputField.text = "Incorrect contents.\nPlease try again.\n";
            myInputField.text += "Reminder: ";
            myInputField.text += GlobalChemistryData.instance.molesOfSubstanceOne + GlobalChemistryData.instance.substanceOne + " + ";
            myInputField.text += GlobalChemistryData.instance.molesOfSubstanceTwo + GlobalChemistryData.instance.substanceTwo;
            myInputField.text += " -> " + GlobalChemistryData.instance.molesOfSubstanceResult + GlobalChemistryData.instance.substanceResult;
        }
        else
        {
            myInputField.text = "Amounts:\n";
            if (GlobalChemistryData.instance.mixedChemicalOneAmount != 0)
                myInputField.text += GlobalChemistryData.instance.mixedChemicalOne + " - " + (GlobalChemistryData.instance.mixedChemicalOneAmount * 10).ToString("F2") + " moles" + "\n";
            if (GlobalChemistryData.instance.mixedChemicalTwoAmount != 0)
                myInputField.text += GlobalChemistryData.instance.mixedChemicalTwo + " - " + (GlobalChemistryData.instance.mixedChemicalTwoAmount * 10).ToString("F2") + " moles";
        }
    }
}
