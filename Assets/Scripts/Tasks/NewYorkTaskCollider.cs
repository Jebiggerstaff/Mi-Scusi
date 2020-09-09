using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewYorkTaskCollider : MonoBehaviour
{
    public NewYorkTaskManager NewYorkTaskManager;

    public void Start()
    {
        NewYorkTaskManager = GameObject.Find("NewYorkUI").GetComponent<NewYorkTaskManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.name == "APR_Head" && name=="SSSTag")
        {  
            //Replace with Scusi Man Tag
            NewYorkTaskManager.SSStagsRemoved++;
            if(NewYorkTaskManager.SSStagsRemoved >= NewYorkTaskManager.SSSTags.Length)
            {
                NewYorkTaskManager.DefaceSSSTags = true;
                NewYorkTaskManager.TaskCompleted();
            }
            Destroy(this.gameObject);
        }
    }
}
