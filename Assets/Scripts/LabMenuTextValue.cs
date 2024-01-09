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
        else
        {
            myInputField.text = "Amounts:\n";
            if (GlobalChemistryData.instance.mixedChemicalOneAmount != 0)
                myInputField.text += GlobalChemistryData.instance.mixedChemicalOne + " - " + (GlobalChemistryData.instance.mixedChemicalOneAmount * 10).ToString() + " moles" + "\n";
            if (GlobalChemistryData.instance.mixedChemicalTwoAmount != 0)
                myInputField.text += GlobalChemistryData.instance.mixedChemicalTwo + " - " + (GlobalChemistryData.instance.mixedChemicalTwoAmount * 10).ToString() + " moles";
        }
    }
}
