using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHandContact : MonoBehaviour
{
    [Header("Hand Information")]
    public bool leftHand;
    public bool rightHand;


    [Header("Unity Stuff")]
    public ArmControls arms;

    //Internal Use Only
    bool reaching;
    FixedJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reachCheck();
    }

    void reachCheck()
    {
        if(leftHand)
        {
            reaching = arms.reachingLeft;
        }
        else if(rightHand)
        {
            reaching = arms.reachingRight;
            
        }

        if(!reaching)
        {
            if(joint != null)
            {

                joint.connectedBody = null;
                Destroy(joint);
                joint = null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Grabbing
        if(joint == null)
        {
            var otherRB = collision.gameObject.GetComponent<Rigidbody>();
            if (otherRB != null)
            {
                if (collision.gameObject.tag == "CanBeGrabbed")
                {
                    gameObject.AddComponent<FixedJoint>();
                    joint = GetComponent<FixedJoint>();
                    joint.breakForce = Mathf.Infinity;
                    joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                }
            }
        }
        
        
    }

}
