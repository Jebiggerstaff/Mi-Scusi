 using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class APRController : MonoBehaviour
{
    #region Variables
    private MiScusiActions controls;
    public Transform RespawnPoint;

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
    public bool canPunch = true, poweringUpR = false, poweringUpL = false;
    public float punchForce = 15f;
    private float leftPunchTimer = 0f;
    private float rightPunchTimer = 0f;
    private float punchDelay = 2f;
    private float punchPowerUp = 0f;
    public float powerRight = 0f;
    public float powerLeft = 0f;
    public ParticleSystem PunchR1, PunchR2, PunchR3, PunchR4, PunchR5, PunchL1, PunchL2, PunchL3, PunchL4, PunchL5;

    //quip
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
    public AudioSource quipAudioSource;

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
    MouseYAxisArms, MouseXAxisArms, MouseYAxisBody;
    
	private bool 
    WalkForward,
    StepRight, StepLeft, Alert_Leg_Right,
    Alert_Leg_Left,  GettingUp,
    ResetPose, isRagdoll, isKeyDown, moveAxisUsed,
    jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;
    
    //[HideInInspector]
    public bool 
    jumping, isJumping, inAir,
    punchingRight, punchingLeft;
    
    [HideInInspector]
    public Camera cam;
    private Vector3 Direction;
    
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
    float resetTimer = 0f;



    //Internal Use
    float hasntBentCount = 0;
    #endregion

    //---Setup---//
    void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
        PlayerSetup();
        //this is for power punch
    }

    void Update()
    {
        if(Time.timeScale > 0.1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (canPunch)
        {
            PlayerPunch();          
        }

        leftPunchTimer += .1f;
        rightPunchTimer += .1f;

        PlayerReach();
        
        if(useStepPrediction)
        {
            StepPrediction();
        }
        
        if(!useStepPrediction)
        {
            ResetWalkCycle();
        }

        Reset();

        tryQuip();
    }
  
    //---Fixed Updates---//
    void FixedUpdate()
    {
        Walking();

      //  if (!inAir)
        //{
            PlayerMovement();

        //}

        PlayerRotation();
        ResetPlayerPose();
            
        PlayerGetUpJumping();

    }

    //---Player Setup--//
    void PlayerSetup()
    {

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


        cantgrabmmove = false;
    }  

	//Step Prediction
	void StepPrediction()
	{
        ResetWalkCycle();
		
		//Check direction to walk when off balance
        //Forward
        if (COMP.position.z > APR_Parts[11].transform.position.z && COMP.position.z > APR_Parts[12].transform.position.z)
            WalkForward = true;
        else
            if(!isKeyDown)
				WalkForward = false;
	}
    
    //Reset Walk Cycle
    void ResetWalkCycle()
    {
        //Reset variables when not moving
        if(!WalkForward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }
    }
    
    //Player Movement
    void PlayerMovement()
    {
        if (!knockedOut)
        {
                Direction = cam.transform.rotation * new Vector3(controls.Player.MoveX.ReadValue<float>(), 0.0f, controls.Player.MoveY.ReadValue<float>());
                Direction.y = 0f;
                APR_Parts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(APR_Parts[0].transform.GetComponent<Rigidbody>().velocity, (Direction * moveSpeed) + new Vector3(0, APR_Parts[0].transform.GetComponent<Rigidbody>().velocity.y, 0), 0.8f);

                if (controls.Player.MoveX.ReadValue<float>() != 0 || controls.Player.MoveY.ReadValue<float>() != 0)
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
        if(controls.Player.MoveX.ReadValue<float>() == 0 && controls.Player.MoveY.ReadValue<float>() == 0 && !knockedOut )
        {
            ResetLeftLeg();
            ResetRightLeg();

            //feet force down
            FeetForceDown(APR_Parts[11], APR_Parts[12]);

        }
    }

    //Player Rotation
    void PlayerRotation()
    {
        //if (!inAir)
        //{
            //keyboard movement
            if (!cantgrabmmove)
            {
                if (controls.Player.MoveX.ReadValue<float>() != 0f || controls.Player.MoveY.ReadValue<float>() != 0f)
                {
                    Quaternion Rotation = Quaternion.LookRotation(APR_Parts[0].GetComponent<Rigidbody>().velocity);
                    Rotation.x = 0;
                    APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Inverse(Rotation);
                }
            }

        //}
    }
    
    //Player GetUp & Jumping
    void PlayerGetUpJumping()
	{
        if(controls.Player.Jump.triggered && !knockedOut)
        {
            if(!jumpAxisUsed)
            {
                if(!inAir)
                {
                    JumpParticle.Play();
                    jumping = true;

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
    
    //Player Landed
    public void PlayerLanded()
    {
        if(inAir && !isJumping && !jumping)
        {
            inAir = false;
            HitParticle.Play();
            ResetPose = true;
        }
    }
    
    //Player Reach
    void PlayerReach()
    {
        Bend();

        //Reach Left
            reachLeftAxisUsed = Reach(APR_Parts[5], APR_Parts[6], reachLeftAxisUsed, new Quaternion(-0.8f - (MouseYAxisArms), -0.9f - (MouseYAxisArms), -0.8f, 1),controls.Player.LeftGrab.ReadValue<float>(),punchingLeft, LeftHand.gameObject);

        //Reach Right
            reachRightAxisUsed = Reach(APR_Parts[3],APR_Parts[4],reachRightAxisUsed, new Quaternion(0.8f + (MouseYAxisArms), -0.9f - (MouseYAxisArms), 0.8f, 1),controls.Player.RightGrab.ReadValue<float>(),punchingRight, RightHand.gameObject);

        

        leftGrab = reachLeftAxisUsed;
        rightGrab = reachRightAxisUsed;

        isgrabbing = (leftGrab || rightGrab);

        cantgrabmmove = LeftHand.GetComponent<HandContact>().ShouldNotRotate() || RightHand.GetComponent<HandContact>().ShouldNotRotate();

    }
    
    //Player Punch
    void PlayerPunch()
    {
        //RightPunch
        if(rightPunchTimer > punchDelay)
        {
            Punch(false, APR_Parts[1], APR_Parts[3], APR_Parts[4], new Quaternion(0.3f, 0f, 0.5f, 1), new Quaternion(1.6f, 0f, -0.5f, 1),
                new Quaternion(1f, 0.04f, 0f, 1), new Quaternion(0.2f, 0, 0, 1), RightHand, UpperRightArmTarget, LowerRightArmTarget);

        }

        //LeftPunch
        if(leftPunchTimer > punchDelay)
        {
            Punch(true, APR_Parts[1], APR_Parts[5], APR_Parts[6], new Quaternion(-0.3f, 0f, -0.5f, 1), new Quaternion(-1.6f, 0f, 0.5f, 1),
                new Quaternion(-1f, 0.04f, 0f, 1f), new Quaternion(-0.2f, 0, 0, 1), LeftHand, UpperLeftArmTarget, LowerLeftArmTarget);

        }
        
    }
    
    void Punch(bool left, GameObject body, GameObject UpperArm, GameObject LowerArm, Quaternion UpperArmPullRot, Quaternion LowerArmPullRot, Quaternion UpperArmReleaseRot,
        Quaternion LowerArmReleaseRot, Rigidbody hand, Quaternion UpperArmTarget, Quaternion LowerArmTarget)
    {
        bool punchingThisArm;
        float inputValue;
        float punchPower;
        ParticleSystem p1, p2, p3, p4, p5;
        float punchTimer;

        #region Left Right Setup
        if(left)
        {
            punchPower = powerLeft;
            punchingThisArm = punchingLeft;
            inputValue = controls.Player.LeftPunch.ReadValue<float>();
            p1 = PunchL1;
            p2 = PunchL2;
            p3 = PunchL3;
            p4 = PunchL4;
            p5 = PunchL5;
            punchTimer = leftPunchTimer;
        }
        else
        {
            punchPower = powerRight;
            punchingThisArm = punchingRight;
            inputValue = controls.Player.RightPunch.ReadValue<float>();
            p1 = PunchR1;
            p2 = PunchR2;
            p3 = PunchR3;
            p4 = PunchR4;
            p5 = PunchR5;
            punchTimer = rightPunchTimer;
        }
        #endregion

        if (inputValue != 0)
        {

            punchPower += Time.deltaTime;
            if (punchPower >= 1 && p1.isPlaying == false)
                p1.Play();
            if (punchPower >= 3f && p2.isPlaying == false)
                p2.Play();
            if (punchPower >= 5 && p3.isPlaying == false)
                p3.Play();
        }
        else
        {
            p1.Stop(); p2.Stop(); p3.Stop();
        }
        
        if (!punchingThisArm && inputValue != 0 && !knockedOut)
        {
            punchingThisArm = true;
            punchTimer = 0;
            




            //Right hand punch pull back pose
            //body.GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);


            UpperArm.GetComponent<ConfigurableJoint>().targetRotation = UpperArmPullRot;
            LowerArm.GetComponent<ConfigurableJoint>().targetRotation = LowerArmPullRot;
        }

        if (punchingThisArm && inputValue == 0 && !knockedOut)
        {
            punchTimer = 0;
            punchingThisArm = false;
            hand.GetComponent<HandContact>().holdPunching(0.45f);

            //Right hand punch release pose
            //body.GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);


            UpperArm.GetComponent<ConfigurableJoint>().targetRotation = UpperArmReleaseRot;
            LowerArm.GetComponent<ConfigurableJoint>().targetRotation = LowerArmReleaseRot;

            //Right hand punch force
           hand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            body.GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

            StartCoroutine(DelayCoroutine());
            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
                if (left)
                {
                    inputValue = controls.Player.LeftPunch.ReadValue<float>();
                }
                else
                {
                    inputValue = controls.Player.RightPunch.ReadValue<float>();
                }
                if (inputValue == 0)
                {
                    UpperArm.GetComponent<ConfigurableJoint>().targetRotation = UpperArmTarget;
                    LowerArm.GetComponent<ConfigurableJoint>().targetRotation = LowerArmTarget;
                }
            }

            punchPower = GetPowerAmount(punchPower);
            if(punchPower > 0)
            {
                Vector3 explosionPos = hand.transform.position;
                int layermask = 1 << 10;
                layermask = ~layermask;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, 50, layermask);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    
                    p4.Play();
                    p5.Play();
                    if (rb != null)
                        rb.AddExplosionForce(punchPower * 1000, explosionPos, 10, 3.0f * (punchPower / 2));
                    if (hit.GetComponent<NewAIMan>() != null)
                        hit.GetComponent<NewAIMan>().Explode(Root.transform.position);
                    if (hit.gameObject.name == "rocket")
                        FindObjectOfType<PentagonTaskManager>().TaskCompleted("Rocket");
                }
                p1.Stop(); p2.Stop(); p3.Stop();
                punchPower = 0;
            }
            
            
            
        }

        //Debug.Log(punchPower);

        //Reassign Values needed
        if (left)
        {
            powerLeft = punchPower;
            punchingLeft = punchingThisArm;
            leftPunchTimer = punchTimer;
        }
        else
        {
            punchingRight = punchingThisArm;
            powerRight = punchPower;
            rightPunchTimer = punchTimer;
        }

    }

    float GetPowerAmount(float power)
    {
        float ret = 0;

        if (power >= 5)
            ret = 10;
        else if (power >= 3)
            ret = 5;
        else if (ret >= 1)
            ret = 2;
        else
            ret = 0;


        return ret;
    }

    //---Getting Punched Functions--//
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
        var sound = GetComponent<SoundOnCollision>();
        if (sound == null)
        {
            sound = GetComponentInChildren<SoundOnCollision>();
        }
        if (sound != null)
        {
            sound.tryPlayAudio(sound.KnockedOut);
        }
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

    //Player Walking
    void Walking()
	{
        if (controls.Player.MoveX.ReadValue<float>() != 0f || controls.Player.MoveY.ReadValue<float>() != 0f)
        {
            if (!inAir && !knockedOut)
            {
                if (WalkForward)
                {
                    //Set up right leg
                    if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
                    {
                        StepRight = true;
                        Alert_Leg_Right = true;
                        Alert_Leg_Left = true;
                    }

                    //Set up left leg
                    if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
                    {
                        StepLeft = true;
                        Alert_Leg_Left = true;
                        Alert_Leg_Right = true;
                    }
                }

                //Step right
                Step(false);
                
                //Step left
                Step(true);
            }
        }
        else
        {
            ResetLeftLeg();
            ResetRightLeg();

            //feet force down
            FeetForceDown(APR_Parts[11], APR_Parts[12]);

            ResetPlayerPose();
        }
    }
     
    //---Activate Ragdoll---//
    public void ActivateRagdoll()
	{
        isRagdoll = true;
		
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

        //Other
        LeftHand.GetComponent<HandContact>().UnGrab();
        RightHand.GetComponent<HandContact>().UnGrab();
        Root.GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Free;
        Root.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
    }

	//---Deactivate Ragdoll---//
	void DeactivateRagdoll()
	{
        isRagdoll = false;
		
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



        Root.GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Locked;
        Root.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Locked;


        ResetPose = true;
	}
    
    //Reset Player Pose
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
    
    //Reaching
    bool Reach(GameObject UpperArm, GameObject LowerArm, bool ReachAxisUsed, Quaternion targetRot, float reach, bool punching, GameObject hand)
    {
        if (!knockedOut && reach != 0 && !punching)
        {
            if (!ReachAxisUsed)
            {
                //Adjust Left Arm joint strength
                UpperArm.GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                UpperArm.GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                LowerArm.GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                LowerArm.GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

                ReachAxisUsed = true;
            }

            //upper  left arm pose
            UpperArm.GetComponent<ConfigurableJoint>().targetRotation = targetRot;
        }
        if (reach == 0 && !punching && !knockedOut)
        {
            if (ReachAxisUsed)
            {
                UpperArm.GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                UpperArm.GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                LowerArm.GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                LowerArm.GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

                ResetPose = true;
                ReachAxisUsed = false;

                hand.GetComponent<HandContact>().UnGrab();
            }
        }
        return ReachAxisUsed;
    }

    //Body Bending
    void Bend()
    {
        if (!knockedOut)
        {
            //values for max rotation for bending

            float limit = 0.8f;
            float bendChangeLimit = 0.8f;
            float mBendMultiplier = 0.00065f;
            float cBendMultiplier = 0.1f;

            float bend = controls.Player.Bend.ReadValue<float>();
            if(Mouse.current != null && controls.Player.Bend.activeControl != null && controls.Player.Bend.activeControl.device == Mouse.current)
            {
                bend *= mBendMultiplier;
            }
            else
            {
                bend *= cBendMultiplier;
            }

            float bendVal = Mathf.Clamp(bend, -bendChangeLimit, bendChangeLimit);
            if(bendVal == 0)
            {
                hasntBentCount += Time.deltaTime;
                if (hasntBentCount >= 2f)
                {
                    MouseYAxisBody = 0;
                    bendVal = 0;
                    hasntBentCount = 0;
                }
            }
            else
            {
                hasntBentCount = 0;
            }

            MouseYAxisBody = Mathf.Clamp(MouseYAxisBody + bendVal, -limit, limit);


            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }

    }

    void ResetLeftLeg()
    {
        //reset to idle LEFT
        ResetToIdle(APR_Parts[9], APR_Parts[10], UpperLeftLegTarget, LowerLeftLegTarget, 7f, 18f);
    }

    void ResetRightLeg()
    {
        //reset to idle RIGHT
        ResetToIdle(APR_Parts[7], APR_Parts[8], UpperRightLegTarget, LowerRightLegTarget, 8f, 17f);
    }

    void ResetToIdle(GameObject UpperLeg,GameObject LowerLeg, Quaternion UpperLegTarget, Quaternion LowerLegTarget, float UpperTimeScale, float LowerTimeScale)
    {
        UpperLeg.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(UpperLeg.GetComponent<ConfigurableJoint>().targetRotation, UpperLegTarget, UpperTimeScale * Time.fixedDeltaTime);
        LowerLeg.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(LowerLeg.GetComponent<ConfigurableJoint>().targetRotation, LowerLegTarget, LowerTimeScale * Time.fixedDeltaTime);
    }

    void FeetForceDown(GameObject LeftFoot,GameObject RightFoot)
    {
        FootForceDown(LeftFoot);
        FootForceDown(RightFoot);
    }

    void FootForceDown(GameObject Foot)
    {
        Foot.GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
    }

    void tryQuip()
    {
        if (currentQuipCooldown <= 0)
        {
            if (controls.Player.Interact.triggered)
            {
                currentQuipCooldown = quipCooldown;
                foreach (var ai in FindObjectsOfType<NewAIMan>())
                {
                    //conditions here?
                    if (!(ai is CrowdAI))
                    {
                        if (Vector3.Distance(Root.transform.position, ai.transform.position) <= quipRange)
                        {
                            bool shouldQuip = true;
                            if (ai is HostileAI)
                            {
                                shouldQuip = !(ai as HostileAI).isAggrod;
                            }
                            else if (ai is SitDownAI)
                            {
                                shouldQuip = !(ai as SitDownAI).sitting;
                            }



                            if (shouldQuip)
                            {
                                ai.getQuipped(quipDuration);
                                Debug.Log("AI quipped");
                            }

                        }
                    }
                }

                if (quipAudioSource != null)
                {
                    quipAudioSource.Play();
                }

            }
        }
        else
        {
            currentQuipCooldown -= Time.deltaTime;
        }

    }

    //Step Controls
    void Step(bool left)
    {
        float StepTimer;
        bool FootStep;
        GameObject TargetFoot;
        GameObject UpperLeg;
        GameObject LowerLeg;
        GameObject OtherLeg;
        Quaternion UpperLegTarget;
        Quaternion LowerLegTarget;
        float timer1, timer2;

        if (left)
        {
            StepTimer = Step_L_timer;
            FootStep = StepLeft;
            TargetFoot = APR_Parts[12];
            UpperLeg = APR_Parts[9];
            LowerLeg = APR_Parts[10];
            OtherLeg = APR_Parts[7];
            UpperLegTarget = UpperLeftLegTarget;
            LowerLegTarget = LowerLeftLegTarget;
            timer1 = 7f;
            timer2 = 18f;
            

        }
        else
        {
            StepTimer = Step_R_timer;
            FootStep = StepRight;
            TargetFoot = APR_Parts[11];
            UpperLeg = APR_Parts[7];
            LowerLeg = APR_Parts[8];
            OtherLeg = APR_Parts[9];
            UpperLegTarget = UpperRightLegTarget;
            LowerLegTarget = LowerRightLegTarget;
            timer1 = 8f;
            timer2 = 17f;
        }

        if (FootStep)
        {
            StepTimer += Time.fixedDeltaTime;

            FootForceDown(TargetFoot);

            //walk simulation
            if (WalkForward)
            {
                UpperLeg.GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(UpperLeg.GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, UpperLeg.GetComponent<ConfigurableJoint>().targetRotation.y, UpperLeg.GetComponent<ConfigurableJoint>().targetRotation.z, UpperLeg.GetComponent<ConfigurableJoint>().targetRotation.w);
                LowerLeg.GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(LowerLeg.GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, LowerLeg.GetComponent<ConfigurableJoint>().targetRotation.y, LowerLeg.GetComponent<ConfigurableJoint>().targetRotation.z, LowerLeg.GetComponent<ConfigurableJoint>().targetRotation.w);

                OtherLeg.GetComponent<ConfigurableJoint>().GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(OtherLeg.GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, OtherLeg.GetComponent<ConfigurableJoint>().targetRotation.y, OtherLeg.GetComponent<ConfigurableJoint>().targetRotation.z, OtherLeg.GetComponent<ConfigurableJoint>().targetRotation.w);
            }

            //step duration
            if (StepTimer > StepDuration)
            {
                StepTimer = 0;
                FootStep = false;

                if (WalkForward)
                {
                    if (left)
                        StepRight = true;
                    else
                        StepLeft = true;
                }
            }
        }
        else
        {
            //reset to idle
            ResetToIdle(UpperLeg, LowerLeg, UpperLegTarget, LowerLegTarget, timer1, timer2);

            //feet force down
            FeetForceDown(APR_Parts[11], APR_Parts[12]);
        }

        if (left)
        {
            Step_L_timer = StepTimer;
            StepLeft = FootStep;
        }
        else
        {
            Step_R_timer = StepTimer;
            StepRight = FootStep;
        }

    }

    public bool GrabbingWithHand(bool left)
    {
        if(left)
        {
            return leftGrab;
        }
        else
        {
            return rightGrab;
        }
    }

    private void Reset()
    {
        
        if (controls.Player.Reset.ReadValue<float>() != 0f)
        { 
            resetTimer = resetTimer + .1f;
            if(resetTimer > 5.5f)
            {

                LeftHand.GetComponent<HandContact>().UnGrab();
                RightHand.GetComponent<HandContact>().UnGrab();
            }
            if (resetTimer > 6f)
            {
                APR_Parts[0].transform.position = RespawnPoint.position;
            }
        }

        else{
            resetTimer = 0;
        }
    }

    public bool IsKnockedOut()
    {
        return knockedOut;
    }
}
