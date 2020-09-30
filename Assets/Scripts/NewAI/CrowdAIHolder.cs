﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrowdAIHolder : AIHolder
{
    public int numPeople;
    public float radius;

    public override NewAIMan MakeAI(GameObject AIMan)
    {
        var ai = base.MakeAI(AIMan);
        (ai as CrowdAI).numCrowd = numPeople;
        (ai as CrowdAI).radius = radius;
        return ai;
    }
}
