﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownAI : NewAIMan
{
    public override void Start()
    {
        base.Start();
        GetComponentInChildren<Animator>().SetBool("Sitting", true);
        foreach(var c in GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(c, sitPlace.GetComponentInParent<Collider>());
        }
        //myRB = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        base.Update();

        if(!offByDistance)
        {

            if (sitting)
            {
                if (agent.enabled == true)
                {
                    disableAgent();
                }
                //if (myRB.useGravity == true)
                //{
                    //myRB.useGravity = false;
                //}
                Vector3 targetSit = sitPlace.position;
                targetSit += new Vector3(0, -0.9f, 0);
                if (transform.position != targetSit)
                {
                    transform.position = targetSit;
                }
                Vector3 targetRot = sitPlace.rotation.eulerAngles;
                if (transform.rotation.eulerAngles != targetRot)
                {
                    transform.rotation = Quaternion.Euler(targetRot);
                }
                anim.SetBool("Sitting", true);




            }
        }

        



    }


    private void OnCollisionEnter(Collision collision)
    {
        if(sitting && !AlwaysSit)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                sitting = false;
                if (GetComponent<Rigidbody>().useGravity == false)
                {
                    GetComponent<Rigidbody>().useGravity = true;
                }
                GetComponentInChildren<Animator>().SetBool("Sitting", false);
                enableAgent();
                foreach (var c in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(c, sitPlace.GetComponentInParent<Collider>(), false);
                }

            }
        }
    }



    //Rigidbody myRB;
    public Transform sitPlace;
    public bool sitting = true;
    public bool AlwaysSit = false;
    
}
