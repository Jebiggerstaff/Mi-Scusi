using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AIHolder
{

    public bool orderedDestinations;
    public int hp;
    public int speed;
    public Vector3 startLocation;
    public List<Vector3> walkLocations;
    public int costume = 0;

    public virtual NewAIMan MakeAI(GameObject AIMan)
    {
        var newAI = UnityEngine.GameObject.Instantiate(AIMan, startLocation, Quaternion.Euler(0, 0, 0)).GetComponent<NewAIMan>();
        newAI.GetComponent<NavMeshAgent>().speed = speed;
        newAI.maxHP = hp;
        newAI.hp = hp;
        newAI.destinations = walkLocations;
        newAI.useRandomDestinations = !orderedDestinations;
        newAI.costume = costume;
        newAI.SetCostume();
        return newAI;

    }

}
