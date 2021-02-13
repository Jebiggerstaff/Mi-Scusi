﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAIMan : MonoBehaviour
{

    public Vector3 currentDestination;

    public bool useRandomDestinations = true;
    public List<Vector3> destinations;
    [HideInInspector]
    public NavMeshAgent agent;
    //Rigidbody rb;
    [HideInInspector]
    public bool grabbedByPlayer = false;
    [HideInInspector]
    public float stunCount;

    public float minimumStopDistance;

    public int maxHP;
    [HideInInspector]
    public int hp;

    [Space]
    public bool shovesPlayer;
    public float shoveForce;
    public float shoveAngle;
    float shoveCooldown = 0;

    [Header("Costumes")]
    public int costume = 0;
    public GameObject[] costumes;

    float percentStayChance = 0.25f;
    float timeToWait = 3f;

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

    List<Vector3> allOverrideDestinations = new List<Vector3>();
    Vector3 overrideDestination;
    readonly Vector3 NO_OVERRIDE_DEST = new Vector3(-99999, -99999, -99999);
    bool beingScooted;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponent<Rigidbody>();
        //anim = GetComponentInChildren<Animator>();
        hp = maxHP;
        currentDest = 0;
        quipped = false;
        needToUpdateDestination = false;
        int legNum = Random.Range(1, 4);
        baseSpeed = agent.speed;
        shoveForce = 750;
        shoveAngle = 30;
        baseAcceleration = agent.acceleration;
        SetCostume();
        SetStopDistance();
        overrideDestination = NO_OVERRIDE_DEST;
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

            if(!quipped)
            {
                agent.acceleration = agent.speed * 0.6f;
            }

            if(allOverrideDestinations.Count > 0)
            {
                overrideDestination = allOverrideDestinations[allOverrideDestinations.Count - 1];
            }
            else
            {
                overrideDestination = NO_OVERRIDE_DEST;
            }

            
            if(shoveCooldown > 0)
            {
                shoveCooldown -= Time.deltaTime;
            }
            if (grabbedByPlayer)
            {
                disableAgent();
                //rb.isKinematic = false;
            }
            else
            {
                if(agent.enabled == true && agent.isOnNavMesh == false)
                {

                    disableAgent();
                    enableAgent();
                }

                if(!(this is SitDownAI && (this as SitDownAI).sitting == true))
                {
                    getnewDest();

                }

                if (agent.enabled == false && stunCount <= 0 && notBeingShoved())
                {
                    enableAgent(); 
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

                if (needToUpdateDestination && stunCount <= 0 && notBeingShoved())
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
                anim.speed = Mathf.Clamp(anim.speed, 0.25f, 2f);
                if (anim != null)
                {

                    if (agent.enabled)
                    {
                        anim.SetFloat("Speed", agent.velocity.magnitude);
                    }
                    else
                    {
                        anim.SetFloat("Speed", 0);
                    }
                }



            }

            

        }

    }

    private bool notBeingShoved()
    {
        if(shovesPlayer)
        {
            return true;
        }
        else
        {
            if(shoveCooldown <= 0)
            {
                return true;
            }
        }
        return false;
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

    public void Explode(Vector3 playerPos)
    {
        if(this is CrowdAI || shovesPlayer)
        {

        }
        else
        {
            StartCoroutine(bump(playerPos));
        }
    }

    IEnumerator bump(Vector3 playerPos)
    {
        if(!( this is SitDownAI && (this as SitDownAI).sitting))
        {
            disableAgent();
            playerPos.y = transform.position.y;

            Vector3 direction = transform.position - playerPos;
            direction.y = Mathf.Tan(10f * Mathf.Deg2Rad) * Mathf.Sqrt((direction.x * direction.x) + (direction.z * direction.z));
            direction = direction.normalized;



            GetComponent<Rigidbody>().AddForce(direction * 25f, ForceMode.Impulse);
            shoveCooldown = 0.5f;

            yield return new WaitForSeconds(1.0f);

            if (stunCount <= 0)
            {

                enableAgent();

            }
        }
        
    }

    IEnumerator playerCollideScoot(float time, Vector3 playerPos)
    {

        StartCoroutine(bump(playerPos));
        yield return new WaitForSeconds(1f);

        float xNeg = 1;
        if (playerPos.x > transform.position.x)
        {
            xNeg = -1;
        }
        float zNeg = 1;
        if(playerPos.z > transform.position.z)
        {
            zNeg = -1;
        }


        Vector3 targetOvr = transform.position + new Vector3(xNeg * Random.Range(5f, 7f), 0, zNeg * Random.Range(5f, 7f));
        allOverrideDestinations.Add(targetOvr);

        yield return new WaitForSeconds(time);

        allOverrideDestinations.Remove(targetOvr);

        
    }


    public void getnewDest()
    {
        if(overrideDestination != NO_OVERRIDE_DEST)
        {
            SetNewDestination(overrideDestination);
        }
        else
        {
            if (useRandomDestinations)
            {
                getnewRandDest();
            }
            else
            {
                getNewOrderedDest();
            }
        }
        
    }

    IEnumerator AddStopDest(Vector3 loc)
    {
        allOverrideDestinations.Add(loc);
        yield return new WaitForSeconds(timeToWait);
        allOverrideDestinations.Remove(loc);
    }

    public void getNewOrderedDest()
    {
       // if(agent.isStopped)
        if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
        {
            if (Random.Range(0f, 1f) <= percentStayChance)
            {
                StartCoroutine(AddStopDest(currentDestination));
            }
            else
            {

                forceGetNewOrderedDest();

            }
        }
    }

    public void getnewRandDest()
    {
        //if(agent.isStopped)
        if (Vector3.Distance(transform.position, currentDestination) < minimumStopDistance)
        {

            if (Random.Range(0f, 1f) <= percentStayChance)
            {
                StartCoroutine(AddStopDest(currentDestination));
            }
            else
            {
                forceGetNewRandDest();
            }
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


        DoCollideThings(collision);



    }

  

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
           
        }
    }


    public void SetNewDestination()
    {
        if(destinations.Count > 0)
            SetNewDestination(destinations[Random.Range(0, destinations.Count)]);
    }
    public void SetNewDestination(Vector3 target)
    {
        if(overrideDestination != NO_OVERRIDE_DEST)
        {
            target = overrideDestination;
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

    public void DoCollideThings(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1") && shovesPlayer && shoveCooldown <= 0)
        {
            Rigidbody root = collision.gameObject.GetComponentInParent<APRController>().Root.GetComponent<Rigidbody>();
            Vector3 direction = root.transform.position - transform.position;
            direction.y = Mathf.Tan(shoveAngle * Mathf.Deg2Rad) * Mathf.Sqrt((direction.x * direction.x) + (direction.z * direction.z));
            direction = direction.normalized;

            float notGroundedAdjustment = 1;
            if (root.GetComponentInParent<APRController>().inAir == true)
            {
                notGroundedAdjustment = 0.35f;
            }

            root.AddForce(direction * shoveForce * notGroundedAdjustment, ForceMode.Impulse);
            shoveCooldown = 0.5f;

            Debug.Log("Shove!");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1") && (!(this is HostileAI)))
        {
            if (agent.enabled == true && shoveCooldown <= 0)
            {

                StartCoroutine(playerCollideScoot(2.0f, collision.gameObject.transform.position));

            }
        }
    }

    public virtual void unPunch()
    {

    }


}
