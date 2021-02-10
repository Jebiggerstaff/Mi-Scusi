﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class HandContact : MonoBehaviour
{
    public APRController APR_Player;
    public ParticleSystem PunchParticle;

    private MiScusiActions controls;

    //Is left or right hand
    public bool Left;

    NewAIMan grabbedAI;

    
    //Have joint/grabbed
	public bool hasJoint;
    

    NewYorkTaskManager NewYorkTaskManager;
    TutorialTaskManager TutorialTaskManager;
    ItalyTaskManager ItalyTaskManager;

    private void Start()
    {
        grabbedAI = null;
        if (SceneManager.GetActiveScene().name == "NewYork")
            NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
        if (SceneManager.GetActiveScene().name == "Tutorial")
            TutorialTaskManager = GameObject.Find("TaskUI").GetComponent<TutorialTaskManager>();
        if (SceneManager.GetActiveScene().name == "Italy")
            ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();
        
    }

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    


    void Update()
    {
        if(APR_Player.isgrabbing == false)
        {
            if (this.gameObject.GetComponent<FixedJoint>())
            {
                this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
            }
            UnGrab();
        }
        
    }

    bool correctLayerTags(Collision col)
    {
        return col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint;
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

        if (correctLayerTags(col))
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
                    if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager.PunchedMafia == false && col.gameObject.name == "MafiaMember")
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
                    ItalyTaskManager.DocumentsCollected++;
                    if (ItalyTaskManager.DocumentsCollected == 5)
                    {
                        ItalyTaskManager.TaskCompleted("Collect5documents");
                    }
                    Destroy(col.gameObject);
                }


                hasJoint = true;


                //APR_Player.leftGrab = true;


                this.gameObject.AddComponent<FixedJoint>();
                this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();


                var ai = col.gameObject.GetComponent<NewAIMan>();
                if (ai != null)
                {
                    ai.grabbedByPlayer = true;
                    grabbedAI = ai;
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
        if(hasJoint)
        {
            if(GetComponent<FixedJoint>().connectedBody.mass >= 10)
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
            this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
            hasJoint = false;
        }


        if (grabbedAI != null)
        {
            grabbedAI.grabbedByPlayer = false;
            grabbedAI = null;
        }


        if (hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
        {
            hasJoint = false;
        }

    }

}
