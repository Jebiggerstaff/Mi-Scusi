using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AIRagdollMan : MonoBehaviour
{
    //Active Ragdoll Player parts
    public GameObject
    //
    Root, Body, Head,
    UpperRightArm, LowerRightArm,
    UpperLeftArm, LowerLeftArm;
    

    //Rigidbody Hands
    public Rigidbody RightHand, LeftHand;

    //Center of mass point
    public Transform COMP;

    [Header("Hand Dependancies")]
    //Hand Controller Scripts & dependancies
    public AIHandContact GrabRight;
    public AIHandContact GrabLeft;

    [Header("The Layer Only This Player Is On")]
    //Player layer name
    public string thisPlayerLayer = "Player_1";

    //Animation
   // public GameObject skin;
    public Animator skinAnim;
    private int LeftPunchHash = Animator.StringToHash("LeftPunch");
    private int RightPunchHash = Animator.StringToHash("RightPunch");
    private int sittinghash;

    [Header("Balance Properties")]
    //Balance
    public bool autoGetUpWhenPossible = true;
    public float balanceHeight = 2.5f;

    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f;
    public float armReachStiffness = 2000f;
    

    [Header("Audio")]
    //Impact sounds
    public float ImpactForce = 10f;
    public AudioClip[] Impacts;
    public AudioClip[] Hits;
    public AudioSource SoundSource;
    
    private bool
    balanced = true, 
    ResetPose, isRagdoll, 
    reachLeftAxisUsed, reachRightAxisUsed;

    [HideInInspector]
    public bool
    jumping, isJumping, inAir,
    punchingRight, punchingLeft, moving = false;

    private Vector3 CenterOfMassPoint;

    //Active Ragdoll Player Parts Array
    private GameObject[] APR_Parts;

    [Header("Player Editor Debug Mode")]
    //Debug
    public bool editorDebugMode;


    float punchTimer;
    [Header("AI Punching")]
    public float punchForce;

    //---Setup---//
    //////////////
    void Awake()
    {
        PlayerSetup();
        //ActivateRagdoll();
    }

    
    //---Updates---//
    ////////////////
    void Update()
    {

    }



    //---Fixed Updates---//
    //////////////////////
    void FixedUpdate()
    {

    }

    //---Player Setup--//
    ////////////////////
    void PlayerSetup()
    {
        //Setup/reroute active ragdoll parts to array
        APR_Parts = new GameObject[]
        {
			//array index numbers
			
			//0
			Root,
			//1
			Body,
			//2
			Head,
			//3
			UpperRightArm,
			//4
			LowerRightArm,
			//5
			UpperLeftArm,
			//6
			LowerLeftArm,
        };
       // skinAnim = skin.GetComponent<Animator>();
    }


    //---Ground Check---//
    /////////////////////
    void GroundCheck()
    {
        Ray ray = new Ray(APR_Parts[0].transform.position, -APR_Parts[0].transform.up);
        RaycastHit hit;

        //Balance when ground is detected
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
        {
            if (!balanced && APR_Parts[0].GetComponent<Rigidbody>().velocity.magnitude < 1f)
            {
                if (autoGetUpWhenPossible)
                {
                    balanced = true;
                }
            }
        }

        //Fall over when ground is not detected
        else if (!Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (balanced)
            {
                balanced = false;
            }
        }


        //Balance on/off
        if (balanced && isRagdoll)
        {
            DeactivateRagdoll();
        }
        else if (!balanced && !isRagdoll)
        {
            ActivateRagdoll();
        }
    }
    
    //---Activate Ragdoll---//
    /////////////////////////
    public void ActivateRagdoll()
    {
        skinAnim.enabled = false;
    }


    //---Deactivate Ragdoll---//
    ///////////////////////////
    void DeactivateRagdoll()
    {
        skinAnim.enabled = true;
    }


    public void AIPunch(bool right)
    {
        StartCoroutine(doAPunch(right));
    }
    IEnumerator doAPunch(bool right)
    {
        if(right)
        {
            punchRight();
            while(punchingRight)
            {
                yield return null;
                punchRight();
            }
        }
        else
        {
            punchLeft();
            while(punchingLeft)
            {
                yield return null;
                punchLeft();
            }
                
        }
    }
    void punchRight()
    {
        //punch right
        if (!punchingRight)
        {
            punchingRight = true;
            punchTimer = 1.0f;
        }

        if (punchTimer <= 0)
        {
            punchTimer = 0;
            punchingRight = false;

            skinAnim.SetTrigger(RightPunchHash);

            //Right hand punch force
            RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            StartCoroutine(DelayCoroutine());
            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);

            }
        }
        else
        {
            punchTimer -= Time.deltaTime;
        }
    }
    public void punchLeft()
    {
        //punch right
        if (!punchingLeft)
        {
            punchingLeft = true;
            punchTimer = 1.5f;
        }

        if (punchTimer <= 0)
        {
            punchTimer = 0;
            punchingLeft = false;

            skinAnim.SetTrigger(LeftPunchHash);

            //Left hand punch force
            LeftHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            StartCoroutine(DelayCoroutine());
            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);

            }
        }
        else
        {
            punchTimer -= Time.deltaTime;
        }
    }


}

