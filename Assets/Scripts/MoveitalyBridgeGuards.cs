using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveitalyBridgeGuards : MonoBehaviour
{
    public Vector3 originalTarget;
    public Vector3 moveTarget;
    StayStillAIMan ai;


    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<StayStillAIMan>();
        ai.destinations[0] = originalTarget;
        ai.currentDestination = originalTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(checkPlayerCostume())
        {
            ai.target = moveTarget;
            ai.shovesPlayer = false;
        }
        else
        {
            ai.target = originalTarget;
            ai.shovesPlayer = true;
        }
    }



    bool checkPlayerCostume()
    {
        //TODO: CHECK PLAYER COSTUME

        return false;
    }
}
