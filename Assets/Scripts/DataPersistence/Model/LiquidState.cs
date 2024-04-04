using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LiquidState
{
    //public string uniqueID = "";

    public string topSubstance = "";
    public string foamSubstance = "";

    public float topSubstanceAmount = 0f;
    public float foamSubstanceAmount = 0f;

    public float fillAmount = 0f;
    public float scaledFillAmount = 0f;

    public Vector3 pos;
}
