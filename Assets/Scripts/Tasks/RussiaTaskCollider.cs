using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussiaTaskCollider : MonoBehaviour
{
    RussiaTaskManager RussiaTaskManager;

    // Start is called before the first frame update
    void Start()
    {
        RussiaTaskManager = FindObjectOfType<RussiaTaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "SDButton" && other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            RussiaTaskManager.TaskCompleted("DestroyFactory");
        }
    }
}
