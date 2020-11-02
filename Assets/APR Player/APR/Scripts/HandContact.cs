using System.Collections;
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

    private void Start()
    {
        grabbedAI = new List<NewAIMan>();
        if (SceneManager.GetActiveScene().name == "NewYork")
            NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
        if (SceneManager.GetActiveScene().name == "Tutorial")
            TutorialTaskManager = GameObject.Find("TaskUI").GetComponent<TutorialTaskManager>();
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

                }

                if(hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
                {
                    hasJoint = false;
                }
            }

        }
    }

    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        if (APR_Player.useControls)
        {
            //Left Hand
            if (Left)
            {
                PunchParticle.Play();
                if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                        
                        //Punch Guy Task
                        if (col.gameObject.GetComponent<NewAIMan>() != null)
                        {
                            if (APR_Player.punchingLeft)
                            {
                                col.gameObject.GetComponent<NewAIMan>().stun(10f);
                                if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Punched == false){
                                    TutorialTaskManager.TaskCompleted("PunchAGuy");
                                    TutorialTaskManager.Punched = true;
                                }
                            }
                        }
                        
                        if (Input.GetAxisRaw(APR_Player.reachLeft) != 0 && !hasJoint && !APR_Player.punchingLeft){
                            //Eat At Cafe Task
                            if (SceneManager.GetActiveScene().name == "NewYork"){
                                if (col.gameObject.name == "CafeFood" && NewYorkTaskManager.AteAtCafe==false){
                                    NewYorkTaskManager.TaskCompleted("EatAtCafe");
                                    NewYorkTaskManager.AteAtCafe = true;
                                }
                            }
                            //Grab Something Task
                            if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Pickedup == false)
                            {
                                TutorialTaskManager.TaskCompleted("GrabSomething");
                                TutorialTaskManager.Pickedup = true;
                            }
                            hasJoint = true;
                            this.gameObject.AddComponent<FixedJoint>();
                            this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                            this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();


                            if (col.gameObject.GetComponent<NewAIMan>() != null)
                            {
                                col.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = true;


                                if (grabbedAI.Contains(col.gameObject.GetComponent<NewAIMan>()) == false)
                                    grabbedAI.Add(col.gameObject.GetComponent<NewAIMan>());

                            }


                        }
                    }

                }

            //Right Hand
            if (!Left)
            {
                PunchParticle.Play();
                if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    

                    if (col.gameObject.GetComponent<NewAIMan>() != null)
                    {
                        if (APR_Player.punchingRight)
                        {
                            col.gameObject.GetComponent<NewAIMan>().stun(10f);
                            if (SceneManager.GetActiveScene().name == "Tutorial" && TutorialTaskManager.Punched == false)
                            {
                                TutorialTaskManager.TaskCompleted("PunchAGuy");
                                TutorialTaskManager.Punched = true;
                            }
                        }
                    }
                    if (Input.GetAxisRaw(APR_Player.reachRight) != 0 && !hasJoint && !APR_Player.punchingRight)
                    {

                        if (SceneManager.GetActiveScene().name == "NewYork")
                        {
                            if (col.gameObject.name == "CafeFood" && NewYorkTaskManager.AteAtCafe == false)
                            {
                                NewYorkTaskManager.TaskCompleted("EatAtCafe");
                                NewYorkTaskManager.AteAtCafe = true;
                            }
                        }
                        if (SceneManager.GetActiveScene().name == "Tutorial"&&TutorialTaskManager.Pickedup==false)
                        {
                            TutorialTaskManager.TaskCompleted("GrabSomething");
                            TutorialTaskManager.Pickedup = true;
                        }

                        hasJoint = true;
                        this.gameObject.AddComponent<FixedJoint>();
                        this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();

                        if (col.gameObject.GetComponent<NewAIMan>() != null)
                        {
                            col.gameObject.GetComponent<NewAIMan>().grabbedByPlayer = true;


                            if (grabbedAI.Contains(col.gameObject.GetComponent<NewAIMan>()) == false)
                                grabbedAI.Add(col.gameObject.GetComponent<NewAIMan>());

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
