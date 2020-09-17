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
    }
    // Start is called before the first frame update
    void Start()
    {
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, currentDestination) < 3)
        {
            SetNewDestination();
        }

        if(grabbedByPlayer)
        {
            agent.enabled = false;
        }
        else
        {
            if(agent.enabled == false && stunCount <= 0)
            {
                agent.enabled = true;
                agent.SetDestination(currentDestination);
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
            if(stunCount <= 0)
            {
                hp = maxHP;
            }
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
        if(collision.collider.GetComponent<APRController>() != null || collision.collider.GetComponentInParent<APRController>() != null)
        {
            agent.enabled = false;

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if ((collision.collider.GetComponent<APRController>() != null || collision.collider.GetComponentInParent<APRController>() != null) && stunCount <= 0)
        {
            agent.enabled = true;
            agent.SetDestination(currentDestination);
        }
    }


    public void SetNewDestination()
    {
        SetNewDestination(destinations[Random.Range(0, destinations.Count)]);
    }
    public void SetNewDestination(Vector3 target)
    {
        currentDestination = target;
        agent.SetDestination(target);
    }

   














    public Vector3 currentDestination;
    public List<Vector3> destinations;
    NavMeshAgent agent;
    Rigidbody rb;
    public bool grabbedByPlayer = false;
    public float stunCount;

    public int maxHP;
    public int hp;
}
