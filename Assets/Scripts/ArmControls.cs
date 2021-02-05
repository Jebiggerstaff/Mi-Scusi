using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControls : MonoBehaviour
{
    [Header("Arms")]
    public ConfigurableJoint rArm;
    public ConfigurableJoint lArm;



    //Internal Use Only
    MiScusiActions controls;
    readonly Quaternion idleTargetRotation  = new Quaternion(0, 0.6f, 0, 1);
    readonly Quaternion rightReachTargetRotation = new Quaternion(1.54f, 1.65f, -1.28f, 1);
    readonly Quaternion leftReachTargetRotation = new Quaternion(-1.54f, 1.65f, 1.28f, 1);

    [HideInInspector]
    public bool reachingLeft;
    [HideInInspector]
    public bool reachingRight;


    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkReaches();
    }

    void checkReaches()
    {
        if (controls.Player.LeftGrab.ReadValue<float>() > 0)
        {
            reachLeft();
        }
        else
        {
            resetReachLeft();
        }
        if (controls.Player.RightGrab.ReadValue<float>() > 0)
        {
            reachRight();
        }
        else
        {
            resetReachRight();
        }
    }

    void reachRight()
    {
        reachingRight = true;
        reach(rArm, rightReachTargetRotation);
    }
    void reachLeft()
    {
        reachingLeft = true;
        reach(lArm, leftReachTargetRotation);
    }
    void resetReachRight()
    {
        reachingRight = false;
        reach(rArm, idleTargetRotation);
    }
    void resetReachLeft()
    {
        reachingLeft = false;
       reach(lArm, idleTargetRotation);
    }

    void reach(ConfigurableJoint arm, Quaternion target)
    {
        arm.targetRotation = target;
    }

    
}
