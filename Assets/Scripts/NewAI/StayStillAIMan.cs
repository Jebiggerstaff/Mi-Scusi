using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayStillAIMan : NewAIMan
{
    public override void Start()
    {
        base.Start();
        minimumStopDistance = 3.5f;
        anim = GetComponentInChildren<Animator>();
    }
    public override void Update()
    {
        base.Update();
        if(!offByDistance)
        {

            SetNewDestination(target);

            if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
            {
                Debug.Log("Standing");

                if (anim != null)
                    anim.SetBool("Running", false);
            }
            else
            {
                Debug.Log("Running");
                if (anim != null)
                    anim.SetBool("Running", true);
            }
        }
    }

    Animator anim;
    public Vector3 target;


}
