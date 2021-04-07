using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonTaskCollider : MonoBehaviour
{
    PentagonTaskManager PentagonTaskManager;

    // Start is called before the first frame update
    void Start()
    {
        PentagonTaskManager = FindObjectOfType<PentagonTaskManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(name == "SaveAliens" && other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            PentagonTaskManager.TaskCompleted("Aliens");
        }
        if(name == "BurningMan" && other.gameObject.name == "BurningPlank")
        {
            PentagonTaskManager.TaskCompleted("BurningMan");
        }
        if(name == "MacaroniSpot" && other.name == "Macaroni")
        {
            PentagonTaskManager.TaskCompleted("Macaroni");
        }
        if(name == "RocketButton" && other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            PentagonTaskManager.TaskCompleted("Rocket");
        }
    }
}
