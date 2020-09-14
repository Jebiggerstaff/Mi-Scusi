using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WindowShatter : MonoBehaviour
{
    public NewYorkTaskManager NewYorkTaskManager;
    public GameObject destroyedVersion;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "NewYork")
            NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CanBeGrabbed")
        {
            NewYorkTaskManager.WindowsBroken++;

            if (NewYorkTaskManager.WindowsBroken == 15)
            {
                NewYorkTaskManager.TaskCompleted("ShatterWindows");
            }

            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }
}
