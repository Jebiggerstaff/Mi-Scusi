using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HostileAIHolder : AIHolder
{
    public bool moveTowardsPlayerOnAggro;
    public float chaseDuration;
    public float maximumAllowedAggroTime;

    public override NewAIMan MakeAI(GameObject AIMan)
    {
        var ai = base.MakeAI(AIMan);
        (ai as HostileAI).moveTowardsPlayerOnAggro = moveTowardsPlayerOnAggro;
        (ai as HostileAI).chaseDuration = chaseDuration;
        (ai as HostileAI).maximumAllowedAggroTime = maximumAllowedAggroTime;
        return ai;
    }
}
