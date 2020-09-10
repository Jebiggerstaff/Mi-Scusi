using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewYorkTaskCollider : MonoBehaviour
{
    public NewYorkTaskManager NewYorkTaskManager;

    static public bool CrossingStreet;

    public void Start()
    {
        NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
    }   

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "APR_Body" && name=="Car")
        {
            Debug.Log("Hit By Car");
            CrossingStreet = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.name == "APR_Head" && name == "SSSTag")
        {
            //Replace with Scusi Man Tag
            NewYorkTaskManager.SSStagsRemoved++;
            if (NewYorkTaskManager.SSStagsRemoved >= NewYorkTaskManager.SSSTags.Length)
            {
                NewYorkTaskManager.DefaceSSSTags = true;
                NewYorkTaskManager.TaskCompleted();
            }
            Destroy(this.gameObject);
        }

        if (other.name == "APR_Head" && name == "LineFront")
        {
            NewYorkTaskManager.CutInLine = true;
            NewYorkTaskManager.TaskCompleted();
        }

        if (other.name == "APR_Head" && name == "RoadS")
        {
            CrossingStreet = true;
            Debug.Log("Crossing Street");
        }
        else if (other.name == "APR_Head" && name == "RoadN")
        {           
            if (CrossingStreet == true)
            {
                NewYorkTaskManager.CrossTheStreet = true;
                NewYorkTaskManager.TaskCompleted();
            }
        }
    }
}
