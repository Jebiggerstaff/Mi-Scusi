using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayStillAIMan : NewAIMan
{
    public override void Start()
    {
        base.Start();
        minimumStopDistance = 3.5f;
        SetStopDistance();
    }
    public override void Update()
    {
        base.Update();
        if(!offByDistance)
        {

            SetNewDestination(target);

            if(agent.isStopped)
            //if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
            {

                if (anim != null)
                    anim.SetBool("Running", false);
            }
            else
            {
                if (anim != null)
                    anim.SetBool("Running", true);
            }
        }
    }
    
    public Vector3 target;


}
