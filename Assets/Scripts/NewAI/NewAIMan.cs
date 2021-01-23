using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAIMan : MonoBehaviour
{

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        hp = maxHP;
        currentDest = 0;
        quipped = false;
        needToUpdateDestination = false;
        int legNum = Random.Range(1, 4);
        baseSpeed = agent.speed;
        baseAcceleration = agent.acceleration;
        if(anim != null)
        {

            anim.SetInteger("RunNumber", legNum);
            anim.SetBool("Running", true);
            anim.SetBool("Sitting", false);
        }
        SetCostume();
        SetStopDistance();
    }
    // Start is called before the first frame update
    public virtual void Start()
    {


        enableAgent();

        forceNewDest();
    }

    // Update is called once per frame
    public virtual void Update()
    {

        if(!offByDistance)
        {

            

            if (grabbedByPlayer)
            {
                disableAgent();
                //rb.isKinematic = false;
            }
            else
            {
                if(agent.enabled == true && agent.isOnNavMesh == false)
                {
                    Debug.Log("Help!  " + gameObject.name);
                    disableAgent();
                    enableAgent();
                }

                if(!(this is SitDownAI && (this as SitDownAI).sitting == true))
                {
                    getnewDest();

                }

                if (agent.enabled == false && stunCount <= 0)
                {
                    enableAgent(); ;
                    //rb.isKinematic = true;
                    if (agent.isOnNavMesh)
                    {

                        agent.SetDestination(currentDestination);

                    }
                    else
                    {
                        needToUpdateDestination = true;
                    }
                }

                if (needToUpdateDestination && stunCount <= 0)
                {
                    enableAgent();
                    //rb.isKinematic = true;
                    if (agent.isOnNavMesh)
                    {
                        agent.SetDestination(currentDestination);
                        needToUpdateDestination = false;
                    }
                    else
                    {
                        disableAgent();
                        //rb.isKinematic = false;
                    }
                }

                if (stunCount > 0)
                {
                    if (stunCount > 30)
                    {
                        stunCount = 30;
                    }
                    disableAgent();
                    //rb.isKinematic = false;
                    stunCount -= Time.deltaTime;


                    if (anim != null)
                        anim.SetBool("Stunned", true);


                    if (stunCount <= 0)
                    {
                        hp = maxHP;
                        if (anim != null)
                            anim.SetBool("Stunned", false);
                    }
                }

                if (quipped)
                {
                    quipTime -= Time.deltaTime;
                    if (quipTime <= 0)
                    {
                        quipped = false;
                        agent.speed = baseSpeed;
                    }

                }


                anim.speed = agent.speed / 3;




            }

            

        }

    }


    public void enableAgent()
    {
        agent.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("AIMan");
        if(GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().isKinematic = true;
    }
    public void disableAgent()
    {
        agent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        if(GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().isKinematic = false;

    }


    public void getnewDest()
    {
        if(useRandomDestinations)
        {
            getnewRandDest();
        }
        else
        {
            getNewOrderedDest();
        }
    }

    public void getNewOrderedDest()
    {
       // if(agent.isStopped)
        if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
        {
            forceGetNewOrderedDest();
        }
    }

    public void getnewRandDest()
    {
        //if(agent.isStopped)
        if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
        {
            forceGetNewRandDest();
        }
    }
    public void forceNewDest()
    {
        if (useRandomDestinations)
        {
            forceGetNewRandDest();
        }
        else
        {
            forceGetNewOrderedDest();
        }

    }
    public void forceGetNewRandDest()
    {
        SetNewDestination();
    }
    public void forceGetNewOrderedDest()
    {
        currentDest++;
        if (currentDest >= destinations.Count)
        {
            currentDest = 0;
        }

        if (destinations.Count > 0)
        {
            SetNewDestination(destinations[currentDest]);
        }
    }

    public void stun(float time)
    {
        hp--;
        if (hp <= 0)
        {

            stunCount += time;
            var sound = GetComponent<SoundOnCollision>();
            if(sound == null)
            {
                sound = GetComponentInChildren<SoundOnCollision>();
            }
            if(sound != null)
            {
                sound.tryPlayAudio(sound.KnockedOut);
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            //rb.isKinematic = false;
        }
    }

  

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            //rb.isKinematic = true;
        }

    }


    public void SetNewDestination()
    {
        if(destinations.Count > 0)
            SetNewDestination(destinations[Random.Range(0, destinations.Count)]);
    }
    public void SetNewDestination(Vector3 target)
    {
        if(currentDestination == target)
        {
            if (!(this is CrowdAI))
            {
                if(anim.GetBool("Running") == true)
                    anim.SetBool("Running", false);

            }
        }
        else
        {
            if(!(this is CrowdAI))
            {
                if (anim.GetBool("Running") == false)
                    anim.SetBool("Running", true);
            }
        }
        if (quipped)
        {
            currentDestination = quipTarget;
        }
        else
        {
            currentDestination = target;
        }
        if (agent.enabled == true && agent.isOnNavMesh)
            agent.SetDestination(currentDestination);
    }

   
    public void getQuipped(float time)
    {
        quipped = true;
        quipTime = time;

        

        Vector3 normalized = (transform.position - FindObjectOfType<APRController>().Root.transform.position).normalized;
        normalized = Vector3.Cross(normalized, Vector3.up);
        var l = Random.Range(0, 2) == 0;
        if(!l)
        {
            normalized = -normalized;
        }
        quipTarget = FindObjectOfType<APRController>().Root.transform.position + (normalized*quipDistance);

        agent.speed = 7;
        agent.acceleration = 5;



        SetNewDestination(quipTarget);
    }


    public void SetCostume()
    {
        if(costumes.Length > 0)
        {

            GameObject go = costumes[0];

            if(costume >= 0 && costume < costumes.Length)
            {
                go = costumes[Random.Range(0,costumes.Length)];
            }

            SwitchCostume(go);

        }
    }
    void SwitchCostume(GameObject go)
    {
        if(go != null)
        {

            foreach (var g in costumes)
            {
                g.SetActive(false);
            }
            go.SetActive(true);
        }
    }




    public void SetStopDistance()
    {
        //agent.stoppingDistance = minimumStopDistance;
    }





    public Vector3 currentDestination;

    public bool useRandomDestinations = true;
    public List<Vector3> destinations;
    [HideInInspector]
    public NavMeshAgent agent;
    //Rigidbody rb;
    public bool grabbedByPlayer = false;
    public float stunCount;

    public float minimumStopDistance;

    public int maxHP;
    public int hp;

    [Header("Costumes")]
    public int costume = 0;
    public GameObject[] costumes;



    bool needToUpdateDestination = false;

    int currentDest;

    [HideInInspector]
    public bool quipped;
    float quipTime;
    float quipDistance = 10f;
    Vector3 quipTarget;

    float baseSpeed;
    float baseAcceleration;
    [HideInInspector]
    public bool offByDistance = false;
    public Animator anim;
    public Collider myCol;
}
