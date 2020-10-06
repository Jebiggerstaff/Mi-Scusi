﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WindowShatter : MonoBehaviour
{
    public NewYorkTaskManager NewYorkTaskManager;
    public GameObject destroyedVersion;

    public int platenum = 0;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "NewYork")
            NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CanBeGrabbed")
        {
            if (platenum == 0)
            {
                if (SceneManager.GetActiveScene().name == "NewYork")
                {
                    NewYorkTaskManager.WindowsBroken++;
                    if (NewYorkTaskManager.WindowsBroken == 15)
                        NewYorkTaskManager.TaskCompleted("ShatterWindows");
                }


                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);

            }


        }

        else if (other.tag == "Ground")
        {
            if (platenum == 1)
            {
                /*
                if (SceneManager.GetActiveScene().name == "NewYork")
                {
                    NewYorkTaskManager.WindowsBroken++;
                    if (NewYorkTaskManager.WindowsBroken == 15)
                        NewYorkTaskManager.TaskCompleted("ShatterWindows");
                }
                */

                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);


            }
        }


    }



    
}
