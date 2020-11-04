 using System.Collections;
using UnityEngine;
using System.Linq;

public class APRController : MonoBehaviour
{

    //particles
    public ParticleSystem JumpParticle;
    public ParticleSystem HitParticle;

    //Active Ragdoll Player parts
    public GameObject
    //
    Root, Body, Head,
	UpperRightArm, LowerRightArm,
	UpperLeftArm, LowerLeftArm, 
	UpperRightLeg, LowerRightLeg,
	UpperLeftLeg, LowerLeftLeg,
	RightFoot, LeftFoot;
	
    //Rigidbody Hands
	public Rigidbody RightHand, LeftHand;
    
    //Center of mass point
	public Transform COMP;
    
    [Header("Hand Dependancies")]
    //Hand Controller Scripts & dependancies
    public HandContact GrabRight;
    public HandContact GrabLeft;
    
    [Header("Input on this player")]
    //Enable controls
    public bool useControls = true;
    public bool usingController = true;
    
    public string forwardBackward;
    public string leftRight;
    public string jump ;
    public string reachLeft;
    public string reachRight;
    public string quip;
    
    [Header("Player Input KeyCodes")]
    //Player KeyCode controls
    public string punchLeft = "q";
    public string punchRight = "e";
    
    [Header("The Layer Only This Player Is On")]
    //Player layer name
    public string thisPlayerLayer = "Player_1";
    
    [Header("Movement Properties")]
    //Movement
    public float moveSpeed = 10f;
    public float turnSpeed = 6f;
    public float jumpForce = 18f;
    
    [Header("Balance Properties")]
    //Balance
    public bool autoGetUpWhenPossible = true;
    public bool useStepPrediction = true;
	public float balanceHeight = 2.5f;
    public float balanceStrength = 5000f;
    public float coreStrength = 1500f;
    public float limbStrength = 500f;
    private bool spinefix = false;
    public float spineTimer;

    //Walking
	public float StepDuration = 0.2f;
	public float StepHeight = 1.7f;
    public float FeetMountForce = 25f;
    
    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f;
    public float armReachStiffness = 2000f;
    public bool leftGrab = false, rightGrab = false;
    public bool isgrabbing = false;
    public bool cantgrabmmove = false;
    
    [Header("Actions")]
    //Punch
    public bool canBeKnockoutByImpact = true;
    public float requiredForceToBeKO = 20f;
    public bool canPunch = true;
    public float punchForce = 15f;
    private float punchTimer = 0f;
    private float punchDelay = 2f;
    public float quipRange = 10f;
    public float quipDuration = 5f;
    public float quipCooldown = 10f;
    private float currentQuipCooldown = 0;
    
    [Header("Audio")]
    //Impact sounds
    public float ImpactForce = 10f;
    public AudioClip[] Impacts;
    public AudioClip[] Hits;
    public AudioSource SoundSource;

    [Header("HP Values")]
    public int maxHP;
    [HideInInspector]
    public int currentHP;
    public float HPPerSecond;
    public float KnockoutTime;
    bool knockedOut;




    //Hidden variables
    private float
    timer, Step_R_timer, Step_L_timer,
    MouseYAxisArms, MouseXAxisArms; 
        public float MouseYAxisBody;
    
	private bool 
    WalkForward, WalkBackward,
    StepRight, StepLeft, Alert_Leg_Right,
    Alert_Leg_Left,  GettingUp,
    ResetPose, isRagdoll, isKeyDown, moveAxisUsed,
    jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;
    
    //[HideInInspector]
    public bool 
    jumping, isJumping, balanced = true, inAir,
    punchingRight, punchingLeft;
    
    [HideInInspector]
    public Camera cam;
    private Vector3 Direction;
	private Vector3 CenterOfMassPoint;
    
    //Active Ragdoll Player Parts Array
	private GameObject[] APR_Parts;
    
    //Joint Drives on & off
	JointDrive
	//
	BalanceOn, PoseOn, CoreStiffness, ReachStiffness, DriveOff;

    //Original pose target rotation
    [HideInInspector]
    public Quaternion
    //
    HeadTarget, BodyTarget,
    UpperRightArmTarget, LowerRightArmTarget,
    UpperLeftArmTarget, LowerLeftArmTarget,
    UpperRightLegTarget, LowerRightLegTarget,
    UpperLeftLegTarget, LowerLeftLegTarget;
    
	[Header("Player Editor Debug Mode")]
	//Debug
	public bool editorDebugMode;

    private string[] joystickNames;

    //---Setup---//
    //////////////
    void Awake()
    {
        ControllerCheck();  
        PlayerSetup();
    }

    void ControllerCheck()
    {
        joystickNames = Input.GetJoystickNames();
        foreach (string joystickName in joystickNames)
            Debug.Log(joystickName);
        if (joystickNames.Length != 0)
        {
            Debug.Log("Controller connected.... switching to controller controls");
            usingController = true;
        }
        else if (joystickNames.Length== 0)
        {
            Debug.Log("No controller found.... using keyboard controls");
            usingController = false;
        }

    }


    void tryQuip()
    {
        if(currentQuipCooldown <= 0)
        {
            if (Input.GetKey(quip))
            {
                Debug.Log("Quipping");
                currentQuipCooldown = quipCooldown;
                foreach(var ai in FindObjectsOfType<NewAIMan>())
                {
                    //conditions here?
                    if(!(ai is CrowdAI))
                    {
                        if(Vector3.Distance(Root.transform.position, ai.transform.position) <= quipRange)
                        {
                            bool shouldQuip = true;
                            if(ai is HostileAI)
                            {
                                shouldQuip = !(ai as HostileAI).isAggrod;
                            }
                            else if(ai is SitDownAI)
                            {
                                shouldQuip = !(ai as SitDownAI).sitting;
                            }



                            if(  shouldQuip )    
                            {
                                ai.getQuipped(quipDuration);
                                Debug.Log("AI quipped");
                            }

                        }
                    }
                }

            }
        }
        else
        {
            currentQuipCooldown -= Time.deltaTime;
        }
        
    }


    void Update()
    {

        if (canPunch && punchTimer > punchDelay)
        {
            PlayerPunch();          
        }

        punchTimer += .1f;

        if (useControls && !inAir)
        {
            PlayerMovement();
            
        }
        
        if(useControls)
        {
            PlayerReach();
        }
        
        if(balanced && useStepPrediction)
        {
            StepPrediction();
            CenterOfMass();
        }
        
        if(!useStepPrediction)
        {
            ResetWalkCycle();
        }
        
        GroundCheck();
        CenterOfMass();
        tryQuip();
    }
    
    
    
    //---Fixed Updates---//
    //////////////////////
    void FixedUpdate()
    {
        Walking();
        
        if(useControls)
        {
            PlayerRotation();
            ResetPlayerPose();
            
            PlayerGetUpJumping();
        }
    }



    //-------------------------------------------------------------
    //--Functions
    //-------------------------------------------------------------



    //---Player Setup--//
    ////////////////////
    void PlayerSetup()
    {
        if (!usingController)
        {
            forwardBackward = "Vertical";
            leftRight = "Horizontal";
            jump = "Jump";
            reachLeft = "Fire1";
            reachRight = "Fire2";
            punchLeft = "q";
            punchRight = "e";
            quip = "f";

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            forwardBackward = "VerticalCon";
            leftRight = "HorizontalCon";
            jump = "JumpCon";
            reachLeft = "Fire1Con";
            reachRight = "Fire2Con";
            punchLeft = "joystick button 4";
            punchRight = "joystick button 5";
            quip = "joystick button 2"; //X button on xbox
        }

        cam = Camera.main;
        
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
        DriveOff.maximumForce = 999999999999;
		
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
			//7
			UpperRightLeg,
			//8
			LowerRightLeg,
			//9
			UpperLeftLeg,
			//10
			LowerLeftLeg,
			//11
			RightFoot,
			//12
			LeftFoot
		};
		
		//Setup original pose for joint drives
        BodyTarget = APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation;
		HeadTarget = APR_Parts[2].GetComponent<ConfigurableJoint>().targetRotation;
		UpperRightArmTarget = APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightArmTarget = APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftArmTarget = APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftArmTarget = APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation;
		UpperRightLegTarget = APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightLegTarget = APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftLegTarget = APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftLegTarget = APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation;

        currentHP = maxHP;
        StartHPRegen();
    }  
    
    
    
    //---Ground Check---//
    /////////////////////
	void GroundCheck()
	{
		Ray ray = new Ray (APR_Parts[0].transform.position, -APR_Parts[0].transform.up);
		RaycastHit hit;
		
		//Balance when ground is detected
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
        {
            if(!balanced && APR_Parts[0].GetComponent<Rigidbody>().velocity.magnitude < 3f)
            {
                if(autoGetUpWhenPossible)
                {
                    balanced = true;
                }
            }
		}
		
		//Fall over when ground is not detected
		else if(!Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")))
		{
            if(balanced)
            {
                balanced = false;
            }
		}

		
		//Balance on/off
		if(balanced && isRagdoll)
		{
            DeactivateRagdoll();
		}
        if (!balanced)
        {
            balanced = true;
        }
    /*
        if(Input.GetButton("Ragdoll"))
            ActivateRagdoll();
	*/
    }
    
    
    
	//---Step Prediction---//
	////////////////////////
	void StepPrediction()
	{
		//Reset variables when balanced
		if(!WalkForward && !WalkBackward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }
		
		//Check direction to walk when off balance
        //Backwards
		if (COMP.position.z < APR_Parts[11].transform.position.z && COMP.position.z < APR_Parts[12].transform.position.z)
        {
            WalkBackward = true;
           
        }
        else
        {
			if(!isKeyDown)
			{
				WalkBackward = false;
               
            }
        }
        
        //Forward
        if (COMP.position.z > APR_Parts[11].transform.position.z && COMP.position.z > APR_Parts[12].transform.position.z)
        {
            WalkForward = true;
        
        }
        else
        {
            if(!isKeyDown)
			{
				WalkForward = false;
            }
        }
	}
    
    
    
    //---Reset Walk Cycle---//
    /////////////////////////
    void ResetWalkCycle()
    {
        //Reset variables when not moving
        if(!WalkForward && !WalkBackward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }
    }
    
    
    
    //---Player Movement---//
    ////////////////////////
    void PlayerMovement()
    {
        if (balanced && !knockedOut)
        {
                Direction = cam.transform.rotation * new Vector3(Input.GetAxisRaw(leftRight), 0.0f, Input.GetAxisRaw(forwardBackward));
                Direction.y = 0f;
                APR_Parts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(APR_Parts[0].transform.GetComponent<Rigidbody>().velocity, (Direction * moveSpeed) + new Vector3(0, APR_Parts[0].transform.GetComponent<Rigidbody>().velocity.y, 0), 0.8f);

                if (Input.GetAxisRaw(leftRight) != 0 || Input.GetAxisRaw(forwardBackward) != 0 && balanced)
                {
                    if (!WalkForward && !moveAxisUsed && !isgrabbing)
                    {
                        WalkForward = true;
                        moveAxisUsed = true;
                        isKeyDown = true;
                    }
                }

        }
        //reseting the legs when you stop moving
        if(Input.GetAxis(leftRight) == 0 && Input.GetAxis(forwardBackward) == 0 && !knockedOut )
        {
            
            //reset to idle LEFT
            APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
            APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

            //feet force down
            APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

            //reset to idle RIGHT
            APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
            APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);

            //feet force down
            APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            
        }
        //Reseting the spine after 10 seconds
        if(Input.GetAxis(leftRight) == 0 && Input.GetAxis(forwardBackward) == 0 && !knockedOut && !usingController && !reachLeftAxisUsed && !reachRightAxisUsed)
        {
            if (spineTimer <= 0)
            {
                spineTimer = 10;
                spinefix = false;
            }
            else
            {
                spineTimer -= Time.deltaTime;
                if (spineTimer <= 0)
                {
                    spinefix = true;
                }
            }

            if (spinefix)
            {
                MouseYAxisBody = 0;
            }
        }
        else
        {
            spineTimer = 10;
        }
    }


    //---Player Rotation---//
    ////////////////////////
    void PlayerRotation()
    {
        if (!inAir)
        {
            //keyboard movement
            if (usingController && !cantgrabmmove)
            {
                if (Input.GetAxisRaw("HorizontalCon") != 0f || Input.GetAxisRaw("VerticalCon") != 0f)
                {
                    Quaternion Rotation = Quaternion.LookRotation(APR_Parts[0].GetComponent<Rigidbody>().velocity);
                    Rotation.x = 0;
                    APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Inverse(Rotation);
                }
            }

            //controller movement
            if (!usingController && !cantgrabmmove)
            {
                if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
                {
                    Quaternion Rotation = Quaternion.LookRotation(APR_Parts[0].GetComponent<Rigidbody>().velocity);
                    Rotation.x = 0;
                    APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Inverse(Rotation);
                }
            }

        }
    }
    
    
    
    //---Player GetUp & Jumping---//
    ///////////////////////////////
    void PlayerGetUpJumping()
	{
        if(Input.GetAxis(jump) > 0 && !knockedOut)
        {
            if(!jumpAxisUsed)
            {
                if(balanced && !inAir)
                {
                    JumpParticle.Play();
                    jumping = true;

                }
                
                else if(!balanced)
                {
                    DeactivateRagdoll();
                }
            }

            jumpAxisUsed = true;
        }
        
        else
        {
            jumpAxisUsed = false;
        }
        
        
		if (jumping && !knockedOut)
        {
            isJumping = true;
                
            var v3 = APR_Parts[0].GetComponent<Rigidbody>().transform.up * jumpForce;
            v3.x = APR_Parts[0].GetComponent<Rigidbody>().velocity.x;
            v3.z = APR_Parts[0].GetComponent<Rigidbody>().velocity.z;
            APR_Parts[0].GetComponent<Rigidbody>().velocity = v3;
        }

		if (isJumping) //deactivates right away
		{
			timer = timer + Time.fixedDeltaTime;
				
			if (timer > 0.2f)
			{
				timer = 0.0f;
				jumping = false;
				isJumping = false;
                inAir = true;
			}
		}
	}
    
    
    
    //---Player Landed---//
    //////////////////////
    public void PlayerLanded()
    {
        if(inAir && !isJumping && !jumping)
        {
            inAir = false;
            HitParticle.Play();
            ResetPose = true;
        }
    }
    
    
    
    //---Player Reach--//
    ////////////////////
    void PlayerReach()
    {
        //Body Bending
        if(!knockedOut)
        {
            //values for max rotation for bending
            if (MouseYAxisBody <= 0.9f && MouseYAxisBody >= -0.9f)
            {
                if(usingController)
                    MouseYAxisBody = Input.GetAxis("joystick Y");
                else
                    MouseYAxisBody = MouseYAxisBody + (Input.GetAxis("Mouse Y") / reachSensitivity);             
            }
            if (MouseYAxisBody > 0.9f)
            {
                MouseYAxisBody = 0.9f;
            }

            else if (MouseYAxisBody < -0.9f)
            {
                MouseYAxisBody = -0.9f;
            }
            
            //Debug.Log(Input.GetAxis("joystick Y"));
            //Debug.Log(MouseYAxisBody);
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }  
            
            
        //Reach Left
        if(!knockedOut && Input.GetAxisRaw(reachLeft) != 0 && !punchingLeft)
        {
            
            if(!reachLeftAxisUsed)
            {
                //Adjust Left Arm joint strength
                APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                    
                //Adjust body joint strength
                //APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
                //APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;
                
                reachLeftAxisUsed = true;
            }
            
            if(MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
            {
                if (usingController)
                    MouseYAxisBody = Input.GetAxis("joystick Y");
                else
                    MouseYAxisBody = MouseYAxisBody + (Input.GetAxis("Mouse Y") / reachSensitivity);              
            }
            
            else if(MouseYAxisArms > 1.2f)
            {
                MouseYAxisArms = 1.2f;
            }
            
            else if(MouseYAxisArms < -1.2f)
            {
                MouseYAxisArms = -1.2f;
            }
            
            //upper  left arm pose
			 APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.8f - (MouseYAxisArms), -0.9f - (MouseYAxisArms), -0.8f, 1);
        }
        
        if(Input.GetAxisRaw(reachLeft) == 0 && !punchingLeft && !knockedOut)
        {
            if(reachLeftAxisUsed)
            {
                if(balanced)
                {
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                
                    //APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    //APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }
                
                else if(!balanced)
                {
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }
                
                ResetPose = true;
                reachLeftAxisUsed = false;
            }
        }
        
        
            
            
        //Reach Right
        if(Input.GetAxisRaw(reachRight) != 0 && !punchingRight && !knockedOut)
        {
            
            if(!reachRightAxisUsed)
            {
                //Adjust Right Arm joint strength
                APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                    
                //Adjust body joint strength
                //APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
                //APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;
                
                reachRightAxisUsed = true;
            }
            
            if(MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
            {
                if (usingController)
                    MouseYAxisBody = Input.GetAxis("joystick Y");
                else
                    MouseYAxisBody = MouseYAxisBody + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            
            else if(MouseYAxisArms > 1.2f)
            {
                MouseYAxisArms = 1.2f;
            }
            
            else if(MouseYAxisArms < -1.2f)
            {
                MouseYAxisArms = -1.2f;
            }
            
            //upper right arm pose
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 0.8f + (MouseYAxisArms), -0.9f - (MouseYAxisArms), 0.8f, 1);
        }
        
        if(Input.GetAxisRaw(reachRight) == 0 && !punchingRight && !knockedOut)
        {
            if(reachRightAxisUsed)
            {
                if(balanced)
                {
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                
                    //APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    //APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }
                
                else if(!balanced)
                {
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }
                
                ResetPose = true;
                reachRightAxisUsed = false;
            }
        }
        
    }
    
    
    
    //---Player Punch---//
    /////////////////////
    void PlayerPunch()
    {
        
        //punch right
        if(!punchingRight && Input.GetKey(punchRight) && !knockedOut)
        {
            punchingRight= true;
            punchTimer = 0;
            
            //Right hand punch pull back pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 0.3f, 0f, 0.5f, 1);
            APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 1.6f, 0f, -0.5f, 1);
		}
        
        if(punchingRight && !Input.GetKey(punchRight) && !knockedOut)
        {
            punchTimer = 0;
            punchingRight = false;
            
            //Right hand punch release pose
			APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
			APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 1f, 0.04f, 0f, 1);
			APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 0.2f, 0, 0, 1);
            
            //Right hand punch force
			RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
 
			APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			
            StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
                if(!Input.GetKey(punchRight))
                {
                    APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
                }
            }
        }
        
        
        //punch left
        if(!punchingLeft && Input.GetKey(punchLeft) && !knockedOut)
        {
            punchTimer = 0;
            punchingLeft = true;
            
            //Left hand punch pull back pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.3f, 0f, -0.5f, 1);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -1.6f, 0f, 0.5f, 1);
            /*APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 0.62f, -0.51f, 0.02f, 1);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -1.31f, 0.5f, 0.5f, 1);*/
        }
        
        if(punchingLeft && !Input.GetKey(punchLeft) && !knockedOut)
        {
            punchTimer = 0;
            punchingLeft = false;
            
            //Left hand punch release pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -1f, 0.04f, 0f, 1f);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.2f, 0, 0, 1);
            /*APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.74f, 0.04f, 0f, 1);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.2f, 0, 0, 1);*/

            //Left hand punch force
            LeftHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
 
            APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			
            StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
                if(!Input.GetKey(punchLeft))
                {
                    APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;
                }
            }
        }
    }
    
    
    
    //---Getting Punched Functions--//
    /////////////////////////////////
    public void GotPunched()
    {
        currentHP--;
        if(currentHP == 0)
        {
            KnockedOut();
        }
        
    }

    private void KnockedOut()
    {
        Debug.Log("Knocked Out!");
        currentHP = 0;
        ActivateRagdoll();
        StopHPRegen();
        StartCoroutine(KnockoutTimer());
    }

    private IEnumerator KnockoutTimer()
    {
        float count = 0;
        autoGetUpWhenPossible = false;
        knockedOut = true;
        while(count < KnockoutTime)
        {
            currentHP = 0;
            balanced = false;
            count += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        knockedOut = false;
        autoGetUpWhenPossible = true;
        Debug.Log("You're back!");
        currentHP = 1;
        DeactivateRagdoll();
        StartHPRegen();
    }

    private void StartHPRegen()
    {
        InvokeRepeating("RegenHP", 1f / HPPerSecond, 1f / HPPerSecond);
    }
    private void StopHPRegen()
    {
        CancelInvoke("RegenHP");
    }
    private void RegenHP()
    {
        if(currentHP < maxHP)
        {
            currentHP++;
        }
    }


    //---Player Walking---//
    ///////////////////////
    void Walking()
	{
        if ((Input.GetAxisRaw("HorizontalCon") != 0f || Input.GetAxisRaw("VerticalCon") != 0f) || (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f))
        {
            if (!inAir && !knockedOut)
            {

                if (WalkForward)
                {
                    //right leg
                    if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
                    {
                        StepRight = true;
                        Alert_Leg_Right = true;
                        Alert_Leg_Left = true;
                    }

                    //left leg
                    if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
                    {
                        StepLeft = true;
                        Alert_Leg_Left = true;
                        Alert_Leg_Right = true;
                    }
                }

                if (WalkBackward)
                {
                    //right leg
                    if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
                    {
                        StepRight = true;
                        Alert_Leg_Right = true;
                        Alert_Leg_Left = true;
                    }

                    //left leg
                    if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
                    {
                        StepLeft = true;
                        Alert_Leg_Left = true;
                        Alert_Leg_Right = true;
                    }
                }

                //Step right
                if (StepRight)
                {
                    Step_R_timer += Time.fixedDeltaTime;

                    //Right foot force down
                    APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

                    //walk simulation
                    if (WalkForward)
                    {
                        APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                        APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.w);

                        APR_Parts[9].GetComponent<ConfigurableJoint>().GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    }

                    if (WalkBackward)
                    {
                        APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * StepHeight, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                        APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * StepHeight * 2, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.w);

                        APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * StepHeight / 2, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    }


                    //step duration
                    if (Step_R_timer > StepDuration)
                    {
                        Step_R_timer = 0;
                        StepRight = false;

                        if (WalkForward || WalkBackward)
                        {
                            StepLeft = true;
                        }
                    }
                }
                else
                {
                    //reset to idle
                    APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
                    APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);

                    //feet force down
                    APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
                    APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
                }


                //Step left

                if (StepLeft)
                {
                    Step_L_timer += Time.fixedDeltaTime;

                    //Left foot force down
                    APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

                    //walk simulation
                    if (WalkForward)
                    {
                        APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                        APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.w);

                        APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    }

                    if (WalkBackward)
                    {
                        APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * StepHeight, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                        APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * StepHeight * 2, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.w);

                        APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * StepHeight / 2, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    }


                    //Step duration
                    if (Step_L_timer > StepDuration)
                    {
                        Step_L_timer = 0;
                        StepLeft = false;

                        if (WalkForward || WalkBackward)
                        {
                            StepRight = true;
                        }
                    }
                }
                else
                {
                    //reset to idle
                    APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
                    APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

                    //feet force down
                    APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
                    APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
                }

            }
        }
        else
        {
            //reset to idle LEFT
            APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
            APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

            //feet force down
            APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

            //reset to idle RIGHT
            APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
            APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);

            //feet force down
            APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

            /* //reset to idle
             APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
             APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

             //feet force down
             APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
             APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse); */
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
        if(!reachRightAxisUsed)
        {
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }
        
        if(!reachLeftAxisUsed)
        {
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }
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
		if(!reachRightAxisUsed)
        {
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }
        
        if(!reachLeftAxisUsed)
        {
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }
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
        
        ResetPose = true;
	}
	
    
    
    //---Reset Player Pose---//
    //////////////////////////
    void ResetPlayerPose()
    {
        if(ResetPose && !jumping && !knockedOut)
        {
             APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = BodyTarget;
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
			APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
			APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;
            
            MouseYAxisArms = 0;
            
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
            APR_Parts[6].GetComponent<Rigidbody>().mass * APR_Parts[6].transform.position +
			APR_Parts[7].GetComponent<Rigidbody>().mass * APR_Parts[7].transform.position +
			APR_Parts[8].GetComponent<Rigidbody>().mass * APR_Parts[8].transform.position +
			APR_Parts[9].GetComponent<Rigidbody>().mass * APR_Parts[9].transform.position +
			APR_Parts[10].GetComponent<Rigidbody>().mass * APR_Parts[10].transform.position) 
            
            /
			
            (APR_Parts[0].GetComponent<Rigidbody>().mass + APR_Parts[1].GetComponent<Rigidbody>().mass +
            APR_Parts[2].GetComponent<Rigidbody>().mass + APR_Parts[3].GetComponent<Rigidbody>().mass +
            APR_Parts[4].GetComponent<Rigidbody>().mass + APR_Parts[5].GetComponent<Rigidbody>().mass +
            APR_Parts[6].GetComponent<Rigidbody>().mass + APR_Parts[7].GetComponent<Rigidbody>().mass +
			APR_Parts[8].GetComponent<Rigidbody>().mass + APR_Parts[9].GetComponent<Rigidbody>().mass +
			APR_Parts[10].GetComponent<Rigidbody>().mass);
			
			COMP.position = CenterOfMassPoint;
	}
    
    
}
