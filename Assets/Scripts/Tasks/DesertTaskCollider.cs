using System.Collections;
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
        if(other.name == "StageSpeaker")
        {
            Debug.Log("Yeet");
            DesertTaskManager.speakersBroken++;
            if(DesertTaskManager.speakersBroken == 4)
            {
                DesertTaskManager.TaskCompleted("Music");
            }
        }
       if(name == "RaceWinArea" && other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            if(DesertTaskManager.CanWinRace)
            {
                DesertTaskManager.TaskCompleted("Race");
            }
        }
       if(name == "SubmarineDropoff" && other.gameObject.name == "Gasoline")
        {
            Destroy(other.gameObject);
            DesertTaskManager.GasCollected++;
            if (DesertTaskManager.GasCollected >= 3)
            {
                DesertTaskManager.TaskCompleted("Sailor");
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.name == "StageSpeaker")
        {
            DesertTaskManager.speakersBroken--;
            Debug.Log("UnYeet");
        }

    }
}
