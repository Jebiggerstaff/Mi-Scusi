using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollowClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                foreach(var x in FindObjectsOfType<NavMeshAgent>())
                {
                    x.SetDestination(hit.point);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var x in FindObjectsOfType<NavMeshAgent>())
            {
                x.enabled = !x.enabled;
            }
        }
    }
}
