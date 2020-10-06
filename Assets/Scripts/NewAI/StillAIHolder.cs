using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class StillAIHolder
{
    
    public int hp;
    public int speed;
    public Vector3 target;

    public virtual NewAIMan MakeAI(GameObject AIMan)
    {
        var newAI = UnityEngine.GameObject.Instantiate(AIMan, target, Quaternion.Euler(0, 0, 0)).GetComponent<NewAIMan>();
        newAI.GetComponent<NavMeshAgent>().speed = speed;
        newAI.maxHP = hp;
        newAI.destinations = new List<Vector3>();

        (newAI as StayStillAIMan).target = target;


        return newAI;

    }

}
