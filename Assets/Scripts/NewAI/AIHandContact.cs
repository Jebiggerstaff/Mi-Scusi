using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIHandContact : MonoBehaviour
{
    public AIRagdollMan APR_Player;
    public ParticleSystem PunchParticle;

    //Is left or right hand
    public bool Left;
    
    //Have joint/grabbed
	public bool hasJoint;

    float punchDelay = 0;

    private void Start()
    {
        
    }

    void Update()
    {
        punchDelay -= Time.deltaTime;
    }

    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        if (APR_Player.punchingLeft && Left)
        {

            if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer == LayerMask.NameToLayer("Player_1") && punchDelay <= 0)
            {

                Debug.Log("Punched Player");

                FindObjectOfType<APRController>().GotPunched();

                punchDelay = 1.0f;
            }

        }
        if (APR_Player.punchingRight && !Left)
        {

            if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer == LayerMask.NameToLayer("Player_1") && punchDelay <= 0)
            {

                Debug.Log("Punched Player");

                FindObjectOfType<APRController>().GotPunched();

                punchDelay = 1.0f;
            }

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        

        
    }


}
