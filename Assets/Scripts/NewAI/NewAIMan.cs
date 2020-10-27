using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAIMan : MonoBehaviour
{
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        hp = maxHP;
        currentDest = 0;
        quipped = false;
        needToUpdateDestination = false;
        int legNum = Random.Range(1, 4);
        baseSpeed = agent.speed;
        baseAcceleration = agent.acceleration;
        if(GetComponentInChildren<Animator>() != null)
        {

            GetComponentInChildren<Animator>().SetInteger("RunNumber", legNum);
            GetComponentInChildren<Animator>().SetBool("Running", true);
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        SetNewDestination();
    }

    // Update is called once per frame
    public virtual void Update()
    {


        getnewDest();

        if(grabbedByPlayer)
        {
            agent.enabled = false;
        }
        else
        {
            if(agent.enabled == false && stunCount <= 0)
            {
                agent.enabled = true;
                if (agent.isOnNavMesh)
                {

                    agent.SetDestination(currentDestination);

                }
                else
                {
                    needToUpdateDestination = true;
                }
            }
            
        }

        if(needToUpdateDestination)
        {
            agent.enabled = true;
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(currentDestination);
                needToUpdateDestination = false;
            }
            else
            {
                agent.enabled = false;
            }
        }

        if(stunCount > 0)
        {
            if(stunCount > 30)
            {
                stunCount = 30;
            }
            agent.enabled = false;
            stunCount -= Time.deltaTime;


            if (GetComponentInChildren<Animator>() != null)
                GetComponentInChildren<Animator>().SetBool("Stunned", true);


            if (stunCount <= 0)
            {
                hp = maxHP;
                if (GetComponentInChildren<Animator>() != null)
                    GetComponentInChildren<Animator>().SetBool("Stunned", false);
            }
        }

        if(quipped)
        {
            quipTime -= Time.deltaTime;
            if(quipTime <= 0)
            {
                quipped = false;
                agent.speed = baseSpeed;
            }

        }


        GetComponentInChildren<Animator>().speed = agent.speed / 3;

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
        if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
        {
            forceGetNewOrderedDest();
        }
    }

    public void getnewRandDest()
    {
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
            stunCount += time;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        
    }


    private void OnCollisionExit(Collision collision)
    {
       
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
                if(GetComponentInChildren<Animator>().GetBool("Running") == true)
                    GetComponentInChildren<Animator>().SetBool("Running", false);

            }
        }
        else
        {
            if(!(this is CrowdAI))
            {
                if (GetComponentInChildren<Animator>().GetBool("Running") == false)
                    GetComponentInChildren<Animator>().SetBool("Running", true);
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













    public Vector3 currentDestination;

    public bool useRandomDestinations = true;
    public List<Vector3> destinations;
    [HideInInspector]
    public NavMeshAgent agent;
    Rigidbody rb;
    public bool grabbedByPlayer = false;
    public float stunCount;

    public float minimumStopDistance;

    public int maxHP;
    public int hp;


    bool needToUpdateDestination = false;

    int currentDest;

    [HideInInspector]
    public bool quipped;
    float quipTime;
    float quipDistance = 10f;
    Vector3 quipTarget;

    float baseSpeed;
    float baseAcceleration;
}
