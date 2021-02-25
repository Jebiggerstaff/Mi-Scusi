using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayStillAIMan : NewAIMan
{
    public Vector3 target;

    public override void Start()
    {
        base.Start();
        minimumStopDistance = 3.5f;
        SetStopDistance();

        if(destinations.Contains(target) == false)
        {
            destinations.Add(target);
        }
    }
    public override void Update()
    {
        base.Update();
    }
    


}
