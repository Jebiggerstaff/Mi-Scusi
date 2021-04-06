using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Reflection;

public class HandContact : MonoBehaviour
{
    public APRController APR_Player;
    public ParticleSystem PunchParticle;

    private MiScusiActions controls;

    //Is left or right hand
    public bool Left;

    NewAIMan grabbedAI;
    PoolNoodle grabbedNoodle;
    SuperSoakerOnOff grabbedSoaker;
    ScooterDrive grabbedScooter;
    FixedJoint joint;
    
    //Have joint/grabbed
	public bool hasJoint;
    

    NewYorkTaskManager NewYorkTaskManager;
    TutorialTaskManager TutorialTaskManager;
    ItalyTaskManager ItalyTaskManager;

    Vector3 colliderOriginalSize;
    BoxCollider myCollider;
    bool holdPunch = false;

    private void Start()
    {
        grabbedAI = null;
    }

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
        myCollider = GetComponent<BoxCollider>();
        colliderOriginalSize = myCollider.size;
    }

    


    void Update()
    {
        if(APR_Player == null)
        {
            APR_Player = GetComponentInParent<APRController>();
        }

        if (SceneManager.GetActiveScene().name == "NewYork" && NewYorkTaskManager==null)
            NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
        if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager == null)
            TutorialTaskManager = GameObject.Find("TaskUI").GetComponent<TutorialTaskManager>();
        if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager == null)
            ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();


        if (APR_Player.isgrabbing == false || APR_Player.GrabbingWithHand(Left) == false)
        {
            if (joint != null)
            {
                joint.breakForce = 0;
            }
            UnGrab();
        }


        punchingSizeDifference();
    }
    
    public void holdPunching(float t = 0.4f)
    {
        StartCoroutine(keepPunch(t));
    }
    IEnumerator keepPunch(float t)
    {
        holdPunch = true;
        yield return new WaitForSeconds(t);
        holdPunch = false;
    }
    void punchingSizeDifference()
    {
        bool punchingThisArm;
        float scale;
        if (holdPunch)
        {
            punchingThisArm = true;
            scale = 5;
        }
        else
        {
            scale = 2;
            if (Left)
            {
                punchingThisArm = APR_Player.punchingLeft;

            }
            else
            {
                punchingThisArm = APR_Player.punchingRight;
            }
        }


        if (punchingThisArm)
        {
            myCollider.size = colliderOriginalSize * scale;
        }
        else
        {
            myCollider.size = colliderOriginalSize;
        }
    }

    bool correctLayerTags(Collision col)
    {
        return col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint;
    }
    public bool isPunching()
    {
        bool punchingThisArm;
        if(holdPunch)
        {
            punchingThisArm = true;
        }
        else
        {
            if (Left)
            {
                punchingThisArm = APR_Player.punchingLeft;

            }
            else
            {
                punchingThisArm = APR_Player.punchingRight;
            }
        }
       

        return punchingThisArm;

    }
    void contact(bool left, Collision col)
    {
        bool punchingThisArm;
        float reachValue;
        if (left)
        {
            reachValue = controls.Player.LeftGrab.ReadValue<float>();
            punchingThisArm = APR_Player.punchingLeft;
            
        }
        else
        {
            reachValue = controls.Player.RightGrab.ReadValue<float>();
            punchingThisArm = APR_Player.punchingRight;
        }

        if (correctLayerTags(col) && APR_Player.IsKnockedOut() == false)
        {
            if (col.gameObject.GetComponent<NewAIMan>() != null)
            {
                if (punchingThisArm)
                {
                    PunchParticle.Play();

                    //Hit an AI
                    col.gameObject.GetComponent<NewAIMan>().stun(10f);


                    //Tutorial Punch Task
                    if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Punched == false)
                    {
                        TutorialTaskManager.TaskCompleted("PunchAGuy");
                        TutorialTaskManager.Punched = true;
                    }
                    //Mafia Punch Task
                    if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager.PunchedMafia == false && col.gameObject.name == "HatesOldPeople")
                    {
                        ItalyTaskManager.TaskCompleted("BeatUpMafiaMembers");
                        ItalyTaskManager.PunchedMafia = true;

                    }
                    //Angry Customer Punch Task
                    if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager.PunchedCustomer == false && col.gameObject.name == "AngryCustomer")
                    {
                        ItalyTaskManager.TaskCompleted("KnockoutAngryCustomer");
                        ItalyTaskManager.PunchedCustomer = true;
                    }

                }
            }


            if (reachValue != 0 && !hasJoint && !punchingThisArm)
            {
                //Eat At Cafe Task
                if (SceneManager.GetActiveScene().name == "NewYork" && col.gameObject.name == "CafeFood" && NewYorkTaskManager.AteAtCafe == false)
                {
                    NewYorkTaskManager.TaskCompleted("EatAtCafe");
                    NewYorkTaskManager.AteAtCafe = true;
                }
                //Eat Spaghetti Task
                if (SceneManager.GetActiveScene().name == "Italy" && col.gameObject.name == "SpaghettiBowl" && ItalyTaskManager.AteSpaghetti == false)
                {
                    ItalyTaskManager.TaskCompleted("EatSpaghetti");
                    Destroy(col.gameObject);
                    ItalyTaskManager.AteSpaghetti = true;
                }
                //Grab Something Task
                if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Pickedup == false)
                {
                    TutorialTaskManager.TaskCompleted("GrabSomething");
                    TutorialTaskManager.Pickedup = true;
                }
                //Steal 5 doucments Task
                if (SceneManager.GetActiveScene().name == "Italy" && col.gameObject.name == "Document")
                {
                    Destroy(col.gameObject);
                    ItalyTaskManager.DocumentsCollected++;
                    if (ItalyTaskManager.DocumentsCollected == 5)
                    {
                        ItalyTaskManager.TaskCompleted("Collect5documents");
                    }
                    RandomAudioMaker.makeAudio(FindObjectOfType<ItalyTaskManager>().DocumentGrabbedNoise);
                }
                //Drink Coffee
                if (col.gameObject.name == "CoffeeMug")
                {
                    CoffeeSpeed cof = new CoffeeSpeed();
                    cof.SpeedUp();
                }
                if(SceneManager.GetActiveScene().name == "Office" && col.gameObject.name == "BossStache")
                {
                    var rb = col.gameObject.GetComponent<Rigidbody>();
                    
                    rb.constraints = RigidbodyConstraints.None;

                    FindObjectOfType<OfficeTaskManager>().TaskCompleted("BossStache");
                }


                hasJoint = true;


                //APR_Player.leftGrab = true;


                var ai = col.gameObject.GetComponent<NewAIMan>();
                
                if(!(ai != null && ai.shovesPlayer))
                {

                    joint = this.gameObject.AddComponent<FixedJoint>();
                    joint.breakForce = Mathf.Infinity;
                    joint.connectedBody = col.gameObject.GetComponent<Rigidbody>();

                    if (ai != null)
                    {
                        ai.grabbedByPlayer = true;
                        grabbedAI = ai;
                    }

                }

                var pn = col.gameObject.GetComponentInParent<PoolNoodle>();
                if(pn != null)
                {
                    pn.handCount++;
                    grabbedNoodle = pn;
                }

                var ss = col.gameObject.GetComponent<SuperSoakerOnOff>();
                if(ss != null)
                {
                    ss.handCount++;
                    grabbedSoaker = ss;
                }

                var sd = col.gameObject.GetComponent<ScooterDrive>();
                if (sd != null)
                {
                    sd.driveCount++;
                    grabbedScooter = sd;
                }

                /*
                //too check for grab constraints
                if (col.gameObject.GetComponent<Rigidbody>() && col.gameObject.GetComponent<Rigidbody>().mass >= 10)
                {
                    APR_Player.cantgrabmmove = true;
                }
                if (!col.gameObject.GetComponent<Rigidbody>())
                {
                    APR_Player.cantgrabmmove = true;
                }
                */
            }
        }


    }



    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        contact(this.Left, col);
    }


    public bool ShouldNotRotate()
    {
        if (hasJoint)
        {
            if (joint.connectedBody == null)
            {
                return true;
            }
            if (joint.connectedBody.mass >= 10)
            {
                return true;
            }
            if (joint.connectedBody.isKinematic)
            {
                return true;
            }
        }
        return false;
    }

    public void UnGrab()
    {
        if (hasJoint)
        {
            joint.breakForce = 0;
            hasJoint = false;
        }


        if (grabbedAI != null)
        {
            grabbedAI.grabbedByPlayer = false;
            grabbedAI = null;
        }

        if(grabbedNoodle != null)
        {
            grabbedNoodle.handCount--;
            grabbedNoodle = null;
        }

        if(grabbedSoaker != null)
        {
            grabbedSoaker.handCount--;
            grabbedSoaker = null;
        }

        if(grabbedScooter != null)
        {
            grabbedScooter.driveCount--;
            grabbedScooter = null;
        }


        if (hasJoint && joint == null)
        {
            hasJoint = false;
        }

    }

}
