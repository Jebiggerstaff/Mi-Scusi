using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SitDownAIHolder : AIHolder
{
    public Transform sitPositon;

    public override NewAIMan MakeAI(GameObject AIMan)
    {
        var ai = base.MakeAI(AIMan);
        (ai as SitDownAI).sitPlace = sitPositon;
        return ai;
    }
}
