using System.Collections;
using UnityEngine;

public class HandContact : MonoBehaviour
{
    public APRController APR_Player;
    
    //Is left or right hand
	public bool Left;
    
    //Have joint/grabbed
	public bool hasJoint;
	

    void Update()
    {
        if(APR_Player.useControls)
        {
            //Left Hand
            //On input release destroy joint
            if(Left)
            {
                if(hasJoint && Input.GetAxisRaw(APR_Player.reachLeft) == 0)
                {
                    this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
                    hasJoint = false;
                }

                if(hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
                {
                    hasJoint = false;
                }
            }

            //Right Hand
            //On input release destroy joint
            if(!Left)
            {
                if(hasJoint && Input.GetAxisRaw(APR_Player.reachRight) == 0)
                {
                    this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
                    hasJoint = false;
                }

                if(hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
                {
                    hasJoint = false;
                }
            }
        }
    }

    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        if(APR_Player.useControls)
        {
            //Left Hand
            if(Left)
            {
                if(col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    if (col.transform.parent == true)
                        if (col.transform.parent.name=="AiWander")
                            col.gameObject.GetComponentInParent<AiWander>().GrabbedByPlayer = true;

                    if(col.gameObject.GetComponent<NewAIMan>() != null)
                    {
                        col.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = true;
                        if(APR_Player.punchingLeft)
                        {
                            col.gameObject.GetComponent<NewAIMan>().stun(10f);
                        }
                    }


                    if (Input.GetAxisRaw(APR_Player.reachLeft) != 0 && !hasJoint && !APR_Player.punchingLeft)
                    {
                        hasJoint = true;
                        this.gameObject.AddComponent<FixedJoint>();
                        this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                    }
                }
                
            }

            //Right Hand
            if(!Left)
            {
                if(col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    if (col.transform.parent == true)
                        if (col.transform.parent.name == "AiWander")
                            col.gameObject.GetComponentInParent<AiWander>().GrabbedByPlayer = true;

                    if (col.gameObject.GetComponent<NewAIMan>() != null)
                    {
                        col.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = true;
                        if (APR_Player.punchingRight)
                        {
                            col.gameObject.GetComponent<NewAIMan>().stun(10f);
                        }
                    }

                    if (Input.GetAxisRaw(APR_Player.reachRight) != 0 && !hasJoint && !APR_Player.punchingRight)
                    {
                        hasJoint = true;
                        this.gameObject.AddComponent<FixedJoint>();
                        this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                    }
                }
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.parent==true)
            if (collision.transform.parent.name == "AiWander")
                collision.gameObject.GetComponentInParent<AiWander>().GrabbedByPlayer = false;

        if (collision.gameObject.GetComponent<NewAIMan>() != null)
            collision.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = false;
    }
}
