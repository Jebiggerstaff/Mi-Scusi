using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayStillAIMan : NewAIMan
{
    public override void Start()
    {
        base.Start();
        minimumStopDistance = 2f;
    }
    public override void Update()
    {
        base.Update();
        SetNewDestination(target);

        if(Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
        {
            Debug.Log("Standing");
            
            if (GetComponentInChildren<Animator>() != null)
                GetComponentInChildren<Animator>().SetBool("Running", false);
        }
        else
        {
            Debug.Log("Running");
            if (GetComponentInChildren<Animator>() != null)
                GetComponentInChildren<Animator>().SetBool("Running", true);
        }
    }


    public Vector3 target;


}
