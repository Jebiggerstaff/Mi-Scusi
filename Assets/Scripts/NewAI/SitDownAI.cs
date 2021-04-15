using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownAI : NewAIMan
{
    //Rigidbody myRB;
    public Transform sitPlace;
    public bool sitting = true;
    public bool AlwaysSit = false;


    public override void Start()
    {
        base.Start();
        anim.SetBool("Sitting", true);
        //myRB = GetComponent<Rigidbody>();
        
    }

    public override void Update()
    {

        if(!offByDistance)
        {
            base.Update();

            if (sitting)
            {

                if (sitPlace == null)
                    sitting = false;
                else
                {

                    disableAgent();

                    gameObject.layer = LayerMask.NameToLayer("AIMan");

                    Vector3 targetSit = sitPlace.position;
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
            else
            {

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
                anim.SetBool("Sitting", false);
                enableAgent();

            }
        }
        else
        {
            base.DoCollideThings(collision);
        }
    }

    
}
