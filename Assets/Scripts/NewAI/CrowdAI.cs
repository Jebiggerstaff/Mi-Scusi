using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CrowdAI : NewAIMan
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SpawnLads();
        SetAll();
        StartCoroutine(moveIndividuals());
        agent.isStopped = false;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if(!offByDistance)
        {

            base.getnewDest();

        }
    }

    
    void SpawnLads()
    {
        if (numCrowd > 0)
        {
            GameObject thingToSpawn = AIManPrefab;
            if(hostile)
            {
                thingToSpawn = HostileAIPrefab;
            }

            if(AIMen != null)
            {

                foreach (var c in AIMen)
                {
                    Destroy(c.gameObject);
                }
            }
            AIMen = new List<NewAIMan>();
            for(int i = 0; i < numCrowd; i++)
            {
                float x = Random.Range(-radius, radius);
                float newMax = radius - Mathf.Abs(x);
                float y = Random.Range(-newMax, newMax);

                RaycastHit hit;

                Vector3 target = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);

                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), target - transform.position, out hit, Vector3.Distance(target, transform.position))) ;
                {
                    if (hit.collider != null && (hit.collider.tag == "Buildings" || hit.collider.tag == "AIBlocker") )
                    {
                        target = hit.point;
                    }
                }
                

                var go = Instantiate(thingToSpawn, target, Quaternion.Euler(0, 0, 0));
                AIMen.Add(go.GetComponent<NewAIMan>());
                go.GetComponent<NewAIMan>().minimumStopDistance = 0;
                go.GetComponent<NewAIMan>().SetStopDistance();
                go.GetComponent<NewAIMan>().maxHP = hp;
                go.GetComponent<NewAIMan>().costumeNumber = costumeNumber;
                go.GetComponent<NewAIMan>().SetCostume();
                go.GetComponent<NewAIMan>().shovesPlayer = shovesPlayer;
                if (hostile)
                {
                    go.GetComponent<HostileAI>().moveTowardsPlayerOnAggro = true;
                }
                //go.GetComponent<NewAIMan>().agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            }
        }
    }

    void SetAll()
    {
        foreach(var x in AIMen)
        {
            SetPoint(x);
        }
    }

    IEnumerator moveIndividuals()
    {
        int i = 0;
        while(true)
        {
            
            if(AIMen == null || AIMen.Count == 0)
            {
                i = 0;
                yield return null;
            }
            else
            {
                SetPoint(AIMen[i]);
                i++;
                if(i >= AIMen.Count)
                {
                    i = 0;
                }

                yield return new WaitForSeconds(2f/AIMen.Count);
            }
        }
    }
    
    void SetPoint(NewAIMan man)
    {
        float x = Random.Range(-radius, radius);
        float newMax = radius - Mathf.Abs(x);
        float y = Random.Range(-newMax, newMax);

        RaycastHit hit;

        Vector3 target = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);

        UnityEngine.Debug.DrawLine(transform.position, target, Color.red, 2f);

        if (Physics.Raycast(transform.position + new Vector3(0,1,0), target - transform.position, out hit, Vector3.Distance(target, transform.position)));
        {
           if(hit.collider != null && hit.collider.tag == "Buildings")
            {
                target = hit.point;
            }
        }

        bool shouldSetPoint = true;
        
        if (hostile)
        {
            if( (man as HostileAI).isAggrod )
            {
                man.agent.speed = 9;
                shouldSetPoint = false;
            }
            else
            {
            }
        }


        if(man.quipped == false && shouldSetPoint)
        {

            man.SetNewDestination(target);
            man.agent.speed = Vector3.Distance(man.transform.position, man.currentDestination) / 3;
            man.agent.acceleration = man.agent.speed / 3;

        }
    }


    public float radius;
    public List<NewAIMan> AIMen;
    public bool hostile;

    public GameObject AIManPrefab;
    public GameObject HostileAIPrefab;
    public int numCrowd = -1;
}
