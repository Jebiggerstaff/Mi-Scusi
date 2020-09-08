using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToSetDestination : MonoBehaviour
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            GetComponent<NavMeshAgent>().SetDestination(hit.point);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnMouseDown();
        }
    }

    private void OnMouseDown()
    {
        GetComponent<NavMeshAgent>().isStopped = !GetComponent<NavMeshAgent>().isStopped;
    }

}
