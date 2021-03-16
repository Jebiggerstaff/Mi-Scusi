﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertTaskCollider : MonoBehaviour
{
    DesertTaskManager DesertTaskManager = new DesertTaskManager();
    

    public void Start()
    {
        DesertTaskManager = GameObject.Find("TaskUI").GetComponent<DesertTaskManager>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(gameObject.name == "ShopThing")
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player_1")) 
            {
                DesertTaskManager.ShopThingsMessedWith++;
                if(DesertTaskManager.ShopThingsMessedWith >= 15)
                {
                    Debug.Log("Done!");
                    DesertTaskManager.TaskCompleted("MessUpShop");
                }
                Destroy(this);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.name == "Parts" && name == "AlienShipCollider")
        {
            DesertTaskManager.AlienParts++;
            if(DesertTaskManager.AlienParts == 3)
            {
                DesertTaskManager.TaskCompleted("Aliens");
            }
            Destroy(other.gameObject);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        

    }
}
