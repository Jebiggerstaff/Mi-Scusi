using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTaskCollider : MonoBehaviour
{
    MoonTaskManager MoonTaskManager;

    // Start is called before the first frame update
    void Start()
    {
        MoonTaskManager = FindObjectOfType<MoonTaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "PowerCell")
        {
            MoonTaskManager.PowerCellsRemoved++;
            MoonTaskManager.LightButton();
            Destroy(this);
            Debug.Log("YESSS");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.name == "Button" && other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            MoonTaskManager.TaskCompleted("Button");
        }
    }
}
