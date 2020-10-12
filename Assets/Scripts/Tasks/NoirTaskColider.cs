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
                if (other == suspect && SuspectIdentified == false)
                {
                    SuspectIdentified = true;
                    //have the AI say some shit
                    NoirTaskManager.TaskCompleted("MurderSuspect");
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
    }
}
