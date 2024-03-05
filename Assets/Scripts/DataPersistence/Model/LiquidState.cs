using System;
using System.Collections.Generic;

[Serializable]
public class LiquidState
{
    public int uniqueID = 0;

    public string topSubstance = "";
    public string foamSubstance = "";

    public float topSubstanceAmount = 0f;
    public float foamSubstanceAmount = 0f;

    public float fillAmount = 0f;
    public float scaledFillAmount = 0f;
}
