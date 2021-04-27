using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoWheel : MonoBehaviour
{
 
    public GameObject unhideThisOne;
    public GameObject hideThisOne;
    int Count = 0;

    public GameObject destroyblock;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickaxe" || other.tag == "Gold" || other.tag == "Shovel" )
        {
            Count = Count + 1;
        
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pickaxe" || other.tag == "Gold" || other.tag == "Shovel")
        {
            Count = Count - 1;

        }
    }

    void Update()
    {
        if (Count >= 3)
        {
            if(unhideThisOne != null)
                unhideThisOne.SetActive(true);
            if(hideThisOne != null)
                hideThisOne.SetActive(false);
            if(FindObjectOfType<NewYorkTaskManager>() != null)
            {
                FindObjectOfType<NewYorkTaskManager>().TaskCompleted("777");
            }

            if(destroyblock != null)
                Destroy(destroyblock);
        }

    }
    

  
}
