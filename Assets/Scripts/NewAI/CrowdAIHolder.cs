using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrowdAIHolder : AIHolder
{
    int numPeople;

    public override NewAIMan MakeAI(GameObject AIMan)
    {
        var ai = base.MakeAI(AIMan);
        (ai as CrowdAI).numCrowd = numPeople;
        return ai;
    }
}
