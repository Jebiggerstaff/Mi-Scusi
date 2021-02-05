using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoirTaskColider : MonoBehaviour
{
    public NoirTaskManager NoirTaskManager;

    private bool SuspectIdentified = false;
    private bool WeaponIdentified = false;
    private bool PosterDelivered = false;
    private bool sevensCompleted = false;

    private int sevens=0;

    public void Start()
    {
        NoirTaskManager = GameObject.Find("TaskUI").GetComponent<NoirTaskManager>();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (this.name == "MurderScene")
        {
            foreach (GameObject suspect in NoirTaskManager.Suspects)
            {
                Transform myT = other.gameObject.transform;
                while(myT != null)
                {
                    if (myT.gameObject.name == suspect.name && SuspectIdentified == false)
                    {
                        SuspectIdentified = true;
                        //have the AI say some shit
                        NoirTaskManager.TaskCompleted("MurderSuspect");
                    }
                    myT = myT.parent;
                }
                
            }

            if (other.name == NoirTaskManager.MurderWeapon.name && WeaponIdentified == false)
            {
                WeaponIdentified = true;
                //have the AI say some other shit
                NoirTaskManager.TaskCompleted("MurderWeapon");
            }

            if (other.name == "WantedPoster" && PosterDelivered == false)
            {
                PosterDelivered = true;
                //have the AI say some other shit
                NoirTaskManager.TaskCompleted("WantedPoster");
            }
        }
        if(this.name == "CasinoCollider" && other.name=="Seven")
        {

            sevens++;
            if (sevens >= 3 && sevensCompleted == false){
                sevensCompleted = true;
                NoirTaskManager.TaskCompleted("SevensInCasino");
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (this.name == "CasinoCollider" && other.name == "Seven")
        {

            sevens--;

        }
    }
}
