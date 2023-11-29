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
        myInputField.text = "Amounts:\n";
        myInputField.text += GlobalChemistryData.instance.mixedChemicalOne + " - " + (GlobalChemistryData.instance.mixedChemicalOneAmount*10).ToString() + " moles" + "\n";
        myInputField.text += GlobalChemistryData.instance.mixedChemicalTwo + " - " + (GlobalChemistryData.instance.mixedChemicalTwoAmount*10).ToString() + " moles";
    }
}
