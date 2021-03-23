using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileOnSight : MonoBehaviour
{
    HostileAI ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<HostileAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(ai.transform.position, FindObjectOfType<APRController>().Root.transform.position) <= 30 && ai.stunCount <= 0)
        {
            ai.AggroToPlayer(10000);
        }
    }
}
