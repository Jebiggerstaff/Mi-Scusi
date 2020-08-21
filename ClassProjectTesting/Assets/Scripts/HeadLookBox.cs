using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookBox : MonoBehaviour
{

    public GameObject boxycollider;
    public GameObject player;
   // public GameObject talkingCube;

    private void OnTriggerEnter(Collider other)
    {
      //  talkingCube.SetActive(true);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boxycollider.SetActive(true);
        }
          
        //talkingCube.SetActive(true);
    }


    private void OnTriggerExit(Collider other)
    {
        //talkingCube.SetActive(false);
        boxycollider.SetActive(false);
       
    }

  


}
