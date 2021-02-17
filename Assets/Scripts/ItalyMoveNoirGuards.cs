using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItalyMoveNoirGuards : MonoBehaviour
{
    StayStillAIMan AI;
    public Vector3 moveTarget;

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<StayStillAIMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(checkPlayerCostume())
        {
            if(AI == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                AI.target = moveTarget;
                AI.shovesPlayer = false;
            }
        }
    }




    bool checkPlayerCostume()
    {
        //TODO: CHECK COSTUME

        return false;
    }
}
