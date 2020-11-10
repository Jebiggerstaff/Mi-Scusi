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
    

    [Header("Balance Properties")]
    //Balance
    public bool autoGetUpWhenPossible = true;
    public bool useStepPrediction = true;
    public float balanceHeight = 2.5f;
    public float balanceStrength = 5000f;
    public float coreStrength = 1500f;
    public float limbStrength = 500f;
    //Walking
    public float StepDuration = 0.2f;
    public float StepHeight = 1.7f;
    public float FeetMountForce = 25f;

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

    //Joint Drives on & off
    JointDrive
    //
    BalanceOn, PoseOn, CoreStiffness, ReachStiffness, DriveOff;

    //Original pose target rotation
    Quaternion
    //
    HeadTarget, BodyTarget,
    UpperRightArmTarget, LowerRightArmTarget,
    UpperLeftArmTarget, LowerLeftArmTarget,
    UpperRightLegTarget, LowerRightLegTarget,
    UpperLeftLegTarget, LowerLeftLegTarget;

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
        ActivateRagdoll();
    }

    
    //---Updates---//
    ////////////////
    void Update()
    {

        //GroundCheck();
        //CenterOfMass();


    }



    //---Fixed Updates---//
    //////////////////////
    void FixedUpdate()
    {
        ResetPlayerPose();
    }

    //---Player Setup--//
    ////////////////////
    void PlayerSetup()
    {

        //Setup joint drives
        BalanceOn = new JointDrive();
        BalanceOn.positionSpring = balanceStrength;
        BalanceOn.positionDamper = 0;
        BalanceOn.maximumForce = Mathf.Infinity;

        PoseOn = new JointDrive();
        PoseOn.positionSpring = limbStrength;
        PoseOn.positionDamper = 0;
        PoseOn.maximumForce = Mathf.Infinity;

        CoreStiffness = new JointDrive();
        CoreStiffness.positionSpring = coreStrength;
        CoreStiffness.positionDamper = 0;
        CoreStiffness.maximumForce = Mathf.Infinity;

        ReachStiffness = new JointDrive();
        ReachStiffness.positionSpring = armReachStiffness;
        ReachStiffness.positionDamper = 0;
        ReachStiffness.maximumForce = Mathf.Infinity;

        DriveOff = new JointDrive();
        DriveOff.positionSpring = 25;
        DriveOff.positionDamper = 0;
        DriveOff.maximumForce = Mathf.Infinity;

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

        //Setup original pose for joint drives
        BodyTarget = APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation;
        HeadTarget = APR_Parts[2].GetComponent<ConfigurableJoint>().targetRotation;
        UpperRightArmTarget = APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightArmTarget = APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftArmTarget = APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftArmTarget = APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation;

        /*
        UpperRightLegTarget = APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightLegTarget = APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftLegTarget = APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftLegTarget = APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation;
    */

        
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
        isRagdoll = true;
        balanced = false;

        //Root
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        //head
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        //arms
        if (!reachRightAxisUsed)
        {
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }

        if (!reachLeftAxisUsed)
        {
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }
        /*
        //legs
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        */
    }


    //---Deactivate Ragdoll---//
    ///////////////////////////
    void DeactivateRagdoll()
    {
        isRagdoll = false;
        balanced = true;

        //Root
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularXDrive = BalanceOn;
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularYZDrive = BalanceOn;
        //head
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        //arms
        if (!reachRightAxisUsed)
        {
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }

        if (!reachLeftAxisUsed)
        {
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }
        /*
        //legs
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        */



        ResetPose = true;
    }



    //---Reset Player Pose---//
    //////////////////////////
    void ResetPlayerPose()
    {
        if (ResetPose)
        {
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = BodyTarget;
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
            APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;

            ResetPose = false;
        }
    }



    //---Calculating Center of mass point---//
    /////////////////////////////////////////
    void CenterOfMass()
    {
        CenterOfMassPoint =

        (APR_Parts[0].GetComponent<Rigidbody>().mass * APR_Parts[0].transform.position +
        APR_Parts[1].GetComponent<Rigidbody>().mass * APR_Parts[1].transform.position +
        APR_Parts[2].GetComponent<Rigidbody>().mass * APR_Parts[2].transform.position +
        APR_Parts[3].GetComponent<Rigidbody>().mass * APR_Parts[3].transform.position +
        APR_Parts[4].GetComponent<Rigidbody>().mass * APR_Parts[4].transform.position +
        APR_Parts[5].GetComponent<Rigidbody>().mass * APR_Parts[5].transform.position +
        APR_Parts[6].GetComponent<Rigidbody>().mass * APR_Parts[6].transform.position) //+
        /*
        APR_Parts[7].GetComponent<Rigidbody>().mass * APR_Parts[7].transform.position +
        APR_Parts[8].GetComponent<Rigidbody>().mass * APR_Parts[8].transform.position +
        APR_Parts[9].GetComponent<Rigidbody>().mass * APR_Parts[9].transform.position +
        APR_Parts[10].GetComponent<Rigidbody>().mass * APR_Parts[10].transform.position +
        APR_Parts[11].GetComponent<Rigidbody>().mass * APR_Parts[11].transform.position +
        APR_Parts[12].GetComponent<Rigidbody>().mass * APR_Parts[12].transform.position)
        */
        /

        (APR_Parts[0].GetComponent<Rigidbody>().mass + APR_Parts[1].GetComponent<Rigidbody>().mass +
        APR_Parts[2].GetComponent<Rigidbody>().mass + APR_Parts[3].GetComponent<Rigidbody>().mass +
        APR_Parts[4].GetComponent<Rigidbody>().mass + APR_Parts[5].GetComponent<Rigidbody>().mass +
        APR_Parts[6].GetComponent<Rigidbody>().mass); /*APR_Parts[7].GetComponent<Rigidbody>().mass);
        
        /*
        APR_Parts[8].GetComponent<Rigidbody>().mass + APR_Parts[9].GetComponent<Rigidbody>().mass +
        APR_Parts[10].GetComponent<Rigidbody>().mass + APR_Parts[11].GetComponent<Rigidbody>().mass +
        APR_Parts[12].GetComponent<Rigidbody>().mass);
        */

        COMP.position = CenterOfMassPoint;
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

            //Right hand punch pull back pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.3f, 0f, 0.5f, 1);
            APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(1.6f, 0f, -0.5f, 1);
        }

        if (punchTimer <= 0)
        {
            punchTimer = 0;
            punchingRight = false;

            //Right hand punch release pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(1f, 0.04f, 0f, 1);
            APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.2f, 0, 0, 1);

            //Right hand punch force
            RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            StartCoroutine(DelayCoroutine());
            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
               
                APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
                APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
                
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

            //Right hand punch pull back pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.3f, 0f, -0.5f, 1);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-1.6f, 0f, 0.5f, 1);
        }

        if (punchTimer <= 0)
        {
            punchTimer = 0;
            punchingLeft = false;

            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-1f, 0.04f, 0f, 1f);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.2f, 0, 0, 1);

            //Left hand punch force
            LeftHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            StartCoroutine(DelayCoroutine());
            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);

                APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
                APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;

            }
        }
        else
        {
            punchTimer -= Time.deltaTime;
        }
    }


}

