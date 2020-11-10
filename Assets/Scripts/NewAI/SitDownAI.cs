using System.Collections;
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
                    agent.enabled = false;
                }
                if (GetComponent<Rigidbody>().useGravity == true)
                {
                    GetComponent<Rigidbody>().useGravity = false;
                }
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
                GetComponentInChildren<Animator>().SetBool("Sitting", true);




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
                if (GetComponent<Rigidbody>().useGravity == true)
                {
                    GetComponent<Rigidbody>().useGravity = false;
                }
                GetComponentInChildren<Animator>().SetBool("Sitting", false);
                agent.enabled = true;
                foreach (var c in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(c, sitPlace.GetComponentInParent<Collider>(), false);
                }

            }
        }
    }




    public Transform sitPlace;
    public bool sitting = true;
    public bool AlwaysSit = false;
    
}
