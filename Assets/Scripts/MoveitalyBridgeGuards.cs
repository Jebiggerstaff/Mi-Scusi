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
        if(ai != null)
        {

            ai.destinations[0] = originalTarget;
            ai.currentDestination = originalTarget;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(checkPlayerCostume())
        {
            if(ai != null)
            {
                if(ai.target != moveTarget)
                    ai.target = moveTarget;
                ai.shovesPlayer = false;

            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if(ai != null)
            {
                if(ai.target != originalTarget)
                    ai.target = originalTarget;
                ai.shovesPlayer = true;
                ai.minimumStopDistance = 1f;

            }
        }
    }



    bool checkPlayerCostume()
    {
        //TODO: CHECK PLAYER COSTUME

        return false;
    }
}
