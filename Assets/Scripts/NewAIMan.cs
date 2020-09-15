using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAIMan : MonoBehaviour
{
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
    }



    private void OnCollisionEnter(Collision collision)
    {
        //if collision is player
        agent.enabled = false;

    }


    private void OnCollisionExit(Collision collision)
    {
        //if collision is player
        agent.enabled = true;
        agent.SetDestination(currentDestination);
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
}
