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

            
        }
    }
    
    public Vector3 target;


}
