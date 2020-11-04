﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandContact : MonoBehaviour
{
    public APRController APR_Player;
    public ParticleSystem PunchParticle;

    //Is left or right hand
    public bool Left;

    
    //Have joint/grabbed
	public bool hasJoint;

    //grabbed AI
    public List<NewAIMan> grabbedAI;

    NewYorkTaskManager NewYorkTaskManager;
    TutorialTaskManager TutorialTaskManager;
    ItalyTaskManager ItalyTaskManager;

    private void Start()
    {
        grabbedAI = new List<NewAIMan>();
        if (SceneManager.GetActiveScene().name == "NewYork")
            NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
        if (SceneManager.GetActiveScene().name == "Tutorial")
            TutorialTaskManager = GameObject.Find("TaskUI").GetComponent<TutorialTaskManager>();
        if (SceneManager.GetActiveScene().name == "Italy")
            ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();
        
    }

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

                    foreach(var ai in grabbedAI)
                    {
                        ai.grabbedByPlayer = false;
                    }
                    grabbedAI.Clear();
                    APR_Player.leftGrab = false;
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

                    foreach (var ai in grabbedAI)
                    {
                        ai.grabbedByPlayer = false;
                    }
                    grabbedAI.Clear();
                    APR_Player.rightGrab = false;
                }

                if(hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
                {
                    hasJoint = false;
                }
            }

        }
        if(APR_Player.leftGrab == true || APR_Player.rightGrab == true)
        {
            APR_Player.isgrabbing = true;
        }
        else { APR_Player.isgrabbing = false; APR_Player.cantgrabmmove = false; APR_Player.cantgrabmmove = false; }
    }

    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        if (APR_Player.useControls)
        {
            //Left Hand
            if (Left)
            {
                if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    PunchParticle.Play();
                    if (col.gameObject.GetComponent<NewAIMan>() != null)
                    {
                        if (APR_Player.punchingLeft)
                        {
                            col.gameObject.GetComponent<NewAIMan>().stun(10f);
                            //Tutorial Punch Task
                            if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Punched == false){
                                TutorialTaskManager.TaskCompleted("PunchAGuy");
                                TutorialTaskManager.Punched = true;
                            }
                            //Mafia Punch Task
                            if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager.PunchedMafia == false && col.gameObject.name =="MafiaMember")
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


                    if (Input.GetAxisRaw(APR_Player.reachLeft) != 0 && !hasJoint && !APR_Player.punchingLeft){
                        //Eat At Cafe Task
                        if (SceneManager.GetActiveScene().name == "NewYork"&& col.gameObject.name == "CafeFood" && NewYorkTaskManager.AteAtCafe==false){
                            NewYorkTaskManager.TaskCompleted("EatAtCafe");
                            NewYorkTaskManager.AteAtCafe = true;
                        }
                        //Eat Spaghetti Task
                        if (SceneManager.GetActiveScene().name == "Italy" && col.gameObject.name == "Spaghetti" && ItalyTaskManager.AteSpaghetti == false)
                        {
                            ItalyTaskManager.TaskCompleted("EatSpaghetti");
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
                        APR_Player.leftGrab = true;
                        this.gameObject.AddComponent<FixedJoint>();
                        this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();

                        if (col.gameObject.GetComponent<NewAIMan>() != null)
                        {
                            col.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = true;


                            if (grabbedAI.Contains(col.gameObject.GetComponent<NewAIMan>()) == false)
                                grabbedAI.Add(col.gameObject.GetComponent<NewAIMan>());

                        }
                        //too check for grab constraints
                        if(col.gameObject.GetComponent<Rigidbody>() && col.gameObject.GetComponent<Rigidbody>().mass >= 10)
                        {
                            APR_Player.cantgrabmmove = true;
                        }
                        if (!col.gameObject.GetComponent<Rigidbody>())
                        {
                            APR_Player.cantgrabmmove = true;
                        }

                    }
                }
            }

            //Right Hand
            if (!Left)
            {
                if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    PunchParticle.Play();
                    if (col.gameObject.GetComponent<NewAIMan>() != null)
                    {
                        if (APR_Player.punchingRight)
                        {
                            col.gameObject.GetComponent<NewAIMan>().stun(10f);
                            //Tutorial Punch Task
                            if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Punched == false)
                            {
                                TutorialTaskManager.TaskCompleted("PunchAGuy");
                                TutorialTaskManager.Punched = true;
                            }
                            //Punch Mafia Task
                            if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager.PunchedMafia == false && col.gameObject.name == "MafiaMember")
                            {
                                ItalyTaskManager.TaskCompleted("BeatUpMafiaMembers");
                                ItalyTaskManager.PunchedMafia = true;
                            }
                            //Punch Angry Customer Task
                            if (SceneManager.GetActiveScene().name == "Italy" && ItalyTaskManager.PunchedCustomer == false && col.gameObject.name == "AngryCustomer")
                            {
                                ItalyTaskManager.TaskCompleted("KnockoutAngryCustomer");
                                ItalyTaskManager.PunchedCustomer = true;
                            }
                        }
                    }
                    if (Input.GetAxisRaw(APR_Player.reachRight) != 0 && !hasJoint && !APR_Player.punchingRight)
                    {

                        //Eat At Cafe Task
                        if (SceneManager.GetActiveScene().name == "NewYork" && col.gameObject.name == "CafeFood" && NewYorkTaskManager.AteAtCafe == false)
                        {
                            NewYorkTaskManager.TaskCompleted("EatAtCafe");
                            NewYorkTaskManager.AteAtCafe = true;
                        }
                        //Eat Spaghetti Task
                        if (SceneManager.GetActiveScene().name == "Italy" && col.gameObject.name == "Spaghetti" && ItalyTaskManager.AteSpaghetti == false)
                        {
                            ItalyTaskManager.TaskCompleted("EatSpaghetti");
                            ItalyTaskManager.AteSpaghetti = true;
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
                        //Grab Something Task
                        if (SceneManager.GetActiveScene().name == "Tutorial"&&TutorialTaskManager.Pickedup==false)
                        {
                            TutorialTaskManager.TaskCompleted("GrabSomething");
                            TutorialTaskManager.Pickedup = true;
                        }

                        hasJoint = true;
                        APR_Player.rightGrab = true;
                        this.gameObject.AddComponent<FixedJoint>();
                        this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();

                        if (col.gameObject.GetComponent<NewAIMan>() != null)
                        {
                            col.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = true;


                            if (grabbedAI.Contains(col.gameObject.GetComponent<NewAIMan>()) == false)
                                grabbedAI.Add(col.gameObject.GetComponent<NewAIMan>());

                        }
                        //too check for grab constraints
                        if (col.gameObject.GetComponent<Rigidbody>() && col.gameObject.GetComponent<Rigidbody>().mass >= 10)
                        {
                            APR_Player.cantgrabmmove = true;
                        }
                        if (!col.gameObject.GetComponent<Rigidbody>())
                        {
                            APR_Player.cantgrabmmove = true;
                        }
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

        if (gameObject.GetComponent<FixedJoint>() != null && collision.gameObject.GetComponent<Rigidbody>() != gameObject.GetComponent<FixedJoint>().connectedBody)
        {
            if (collision.gameObject.GetComponent<NewAIMan>() != null)
            {
                collision.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = false;
                grabbedAI.Remove(collision.gameObject.GetComponent<NewAIMan>());
            }
        }

        
    }

}
