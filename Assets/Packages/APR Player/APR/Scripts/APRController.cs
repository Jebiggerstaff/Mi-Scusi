 using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class APRController : MonoBehaviour
{
    #region Variables
    private MiScusiActions controls;

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
    private float punchTimer = 0f;
    private float punchDelay = 2f;
    private float punchPowerUp = 0f;
    public float powerRight = 0f;
    public float powerLeft = 0f;
    public ParticleSystem PunchR1, PunchR2, PunchR3, PunchR4, PunchR5, PunchL1, PunchL2, PunchL3, PunchL4, PunchL5;
    private IEnumerator RP, LP;

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
    jumping, isJumping, balanced = true, inAir,
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

    private string[] joystickNames;
    #endregion

    //---Setup---//
    void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
        PlayerSetup();
        //this is for power punch
        RP = PowerUpTimerRight();
        LP = PowerUpTimerLeft();
    }

    void tryQuip()
    {
        if(currentQuipCooldown <= 0)
        {
            if (controls.Player.Interact.triggered)
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

                if(quipAudioSource != null)
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

    void Update()
    {
        if(Time.timeScale > 0.1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (canPunch && punchTimer > punchDelay)
        {
            PlayerPunch();          
        }

        punchTimer += .1f;
        
        PlayerReach();
        
        if(balanced && useStepPrediction)
        {
            StepPrediction();
        }
        
        if(!useStepPrediction)
        {
            ResetWalkCycle();
        }
        
        //GroundCheck();
        tryQuip();
    }
  
    //---Fixed Updates---//
    void FixedUpdate()
    {
        Walking();

        if (!inAir)
        {
            PlayerMovement();

        }

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
        
    }  
    
    //---Ground Check---//
	void GroundCheck()
	{
		Ray ray = new Ray (APR_Parts[0].transform.position, -APR_Parts[0].transform.up);
		RaycastHit hit;
		
		//Balance when ground is detected
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed
            && !balanced && APR_Parts[0].GetComponent<Rigidbody>().velocity.magnitude < 3f && autoGetUpWhenPossible)
            balanced = true;

		//Fall over when ground is not detected
		else if(!Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")))
            if(balanced)
                balanced = false;

		//Balance on/off
		if(balanced && isRagdoll)
            DeactivateRagdoll();

        if (!balanced)
            balanced = true;
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
        if (balanced && !knockedOut)
        {
                Direction = cam.transform.rotation * new Vector3(controls.Player.MoveX.ReadValue<float>(), 0.0f, controls.Player.MoveY.ReadValue<float>());
                Direction.y = 0f;
                APR_Parts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(APR_Parts[0].transform.GetComponent<Rigidbody>().velocity, (Direction * moveSpeed) + new Vector3(0, APR_Parts[0].transform.GetComponent<Rigidbody>().velocity.y, 0), 0.8f);

                if (controls.Player.MoveX.ReadValue<float>() != 0 || controls.Player.MoveY.ReadValue<float>() != 0 && balanced)
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
            //reset to idle LEFT
            ResetToIdle(APR_Parts[9], APR_Parts[10], UpperLeftLegTarget, LowerLeftLegTarget, 7f, 18f);

            //reset to idle RIGHT
            ResetToIdle(APR_Parts[7], APR_Parts[8], UpperRightLegTarget, LowerRightLegTarget, 8f, 17f);

            //feet force down
            FeetForceDown(APR_Parts[11], APR_Parts[12]);

        }
    }

    //Player Rotation
    void PlayerRotation()
    {
        if (!inAir)
        {
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

        }
    }
    
    //Player GetUp & Jumping
    void PlayerGetUpJumping()
	{
        if(controls.Player.Jump.triggered && !knockedOut)
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
            reachLeftAxisUsed = Reach(APR_Parts[5], APR_Parts[6], reachLeftAxisUsed, new Quaternion(-0.8f - (MouseYAxisArms), -0.9f - (MouseYAxisArms), -0.8f, 1),controls.Player.LeftGrab.ReadValue<float>(),punchingLeft);

        //Reach Right
            reachRightAxisUsed = Reach(APR_Parts[3],APR_Parts[4],reachRightAxisUsed, new Quaternion(0.8f + (MouseYAxisArms), -0.9f - (MouseYAxisArms), 0.8f, 1),controls.Player.RightGrab.ReadValue<float>(),punchingRight);
    }
    
    //---Player Punch---//
    void PlayerPunch()
    {
        
        //punch right
        if (!punchingRight && controls.Player.RightPunch.ReadValue<float>() != 0 && !knockedOut)
        {
            punchingRight= true;
            punchTimer = 0;
            if(poweringUpR == false)
            {
                poweringUpR = true;
                StartCoroutine(RP);
            }
            
            
            //Right hand punch pull back pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 0.3f, 0f, 0.5f, 1);
            APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 1.6f, 0f, -0.5f, 1);
		}
        
        if(punchingRight && controls.Player.RightPunch.ReadValue<float>() == 0 && !knockedOut)
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
                if(controls.Player.RightPunch.ReadValue<float>() == 0)
                {
                    APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
                }
            }
            Vector3 explosionPos = RightHand.transform.position;
            int layermask = 1 << 10;
            layermask = ~layermask;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, 100, layermask);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (hit.GetComponent<NewAIMan>() != null)
                    hit.GetComponent<NewAIMan>().Explode(Root.transform.position);
                PunchR4.Play();
                PunchR5.Play();
                if (rb != null)
                    rb.AddExplosionForce(powerRight * 1000, explosionPos, 10, 3.0f * (powerRight/2));
            }
            StopCoroutine(RP);
            poweringUpR = false;
            powerRight = 0f;
            PunchR1.Stop(); PunchR2.Stop(); PunchR3.Stop();
            RP = PowerUpTimerRight();
        }
        
        
        //punch left
        if(!punchingLeft && controls.Player.LeftPunch.ReadValue<float>() != 0 && !knockedOut)
        {
            punchTimer = 0;
            punchingLeft = true;
            if (poweringUpL == false)
            {
                poweringUpL = true;
                StartCoroutine(LP);
            }

            //Left hand punch pull back pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.3f, 0f, -0.5f, 1);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -1.6f, 0f, 0.5f, 1);
        }
        
        if(punchingLeft && controls.Player.LeftPunch.ReadValue<float>() == 0 && !knockedOut)
        {
            punchTimer = 0;
            punchingLeft = false;
            
            //Left hand punch release pose
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -1f, 0.04f, 0f, 1f);
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.2f, 0, 0, 1);

            //Left hand punch force
            LeftHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
 
            APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			
            StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
                if(controls.Player.LeftPunch.ReadValue<float>() == 0)
                {
                    APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;
                }
            }
            Vector3 explosionPos = LeftHand.transform.position;
            int layermask = 1 << 10;
            layermask = ~layermask;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, 100, layermask);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (hit.GetComponent<NewAIMan>() != null)
                    hit.GetComponent<NewAIMan>().Explode(Root.transform.position);
                
                  
                if (rb != null)
                    rb.AddExplosionForce(powerLeft * 1000, explosionPos, 10, 3.0f * (powerLeft / 2));
            }
            StopCoroutine(LP);
            poweringUpL = false;
            powerLeft = 0f;
            PunchL1.Stop(); PunchL2.Stop(); PunchL3.Stop();
            LP = PowerUpTimerLeft();
        }
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

    private IEnumerator PowerUpTimerRight()
    {
        powerRight = 0f;
        yield return new WaitForSeconds(3);
        PunchR1.Play();
        powerRight = 2f;
        yield return new WaitForSeconds(3);
        PunchR2.Play();
        powerRight = 5f;
        yield return new WaitForSeconds(3);
        PunchR3.Play();
        powerRight = 10f;
    }
    private IEnumerator PowerUpTimerLeft()
    {
        powerLeft = 0f;
        yield return new WaitForSeconds(3);
        PunchL1.Play();
        powerLeft = 2f;
        yield return new WaitForSeconds(3);
        PunchL2.Play();
        powerLeft = 5f;
        yield return new WaitForSeconds(3);
        PunchL3.Play();
        powerLeft = 10f;
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

    //Player Walking
    void Walking()
	{
        if (controls.Player.MoveX.ReadValue<float>() != 0f || controls.Player.MoveY.ReadValue<float>() != 0f)
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

                    //step duration
                    if (Step_R_timer > StepDuration)
                    {
                        Step_R_timer = 0;
                        StepRight = false;

                        if (WalkForward)
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

                    //Step duration
                    if (Step_L_timer > StepDuration)
                    {
                        Step_L_timer = 0;
                        StepLeft = false;

                        if (WalkForward)
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
        }
    }
     
    //---Activate Ragdoll---//
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
    bool Reach(GameObject UpperArm, GameObject LowerArm, bool ReachAxisUsed, Quaternion targetRot, float reach, bool punching)
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
                if (balanced)
                {
                    UpperArm.GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    UpperArm.GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    LowerArm.GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    LowerArm.GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }

                else if (!balanced)
                {
                    UpperArm.GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    UpperArm.GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    LowerArm.GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    LowerArm.GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }

                ResetPose = true;
                ReachAxisUsed = false;
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
            if (MouseYAxisBody <= 0.9f && MouseYAxisBody >= -0.9f)
            {
                MouseYAxisBody = controls.Player.Bend.ReadValue<float>();
            }
            if (MouseYAxisBody > 0.9f)
            {
                MouseYAxisBody = 0.9f;
            }

            else if (MouseYAxisBody < -0.9f)
            {
                MouseYAxisBody = -0.9f;
            }

            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }

    }

    void ResetToIdle(GameObject UpperLeg,GameObject LowerLeg, Quaternion UpperLegTarget, Quaternion LowerLegTarget, float UpperTimeScale, float LowerTimeScale)
    {
        UpperLeg.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(UpperLeg.GetComponent<ConfigurableJoint>().targetRotation, UpperLegTarget, UpperTimeScale * Time.fixedDeltaTime);
        LowerLeg.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(LowerLeg.GetComponent<ConfigurableJoint>().targetRotation, LowerLegTarget, LowerTimeScale * Time.fixedDeltaTime);
    }

    void FeetForceDown(GameObject LeftFoot,GameObject RightFoot)
    {
        LeftFoot.GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
        RightFoot.GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
    }
}
