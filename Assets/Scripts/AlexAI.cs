using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexAI : MonoBehaviour
{
    public GameObject playerRoot;
    public Vector3 lastLoc;
    NewAIMan ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<NewAIMan>();
        playerRoot = FindObjectOfType<APRController>().Root;
        lastLoc = playerRoot.transform.position;
        ai.destinations = new List<Vector3>();
        ai.destinations.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(lastLoc, playerRoot.transform.position) >= 10 || Vector3.Distance(playerRoot.transform.position, transform.position) <= 10)
        {
            Vector3 spot = playerRoot.transform.position + ((transform.position - playerRoot.transform.position).normalized * 50);


            ai.destinations[0] = spot;
            ai.forceNewDest();

            lastLoc = playerRoot.transform.position;
        }

    }
}
