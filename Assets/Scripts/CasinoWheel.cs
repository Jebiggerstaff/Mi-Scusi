using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoWheel : MonoBehaviour
{
 
    public GameObject unhideThisOne;
    public GameObject hideThisOne;
    int Count = 0;

    
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
            unhideThisOne.SetActive(true);
            hideThisOne.SetActive(false);
        }

    }
    

  
}
