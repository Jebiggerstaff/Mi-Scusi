using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    public GameObject TutorialMessgaeObject;
    public GameObject TutorialMessgaeObjectfirstone;
    public GameObject TutorialMessgaeObjectnextone;


    int numPlayerCol;

    private void Update()
    {
        if(Vector3.Distance(transform.position, FindObjectOfType<APRController>().Root.transform.position) <= 9)
        {

            TutorialMessgaeObject.SetActive(true);
        }
        else
        {
            TutorialMessgaeObject.SetActive(false);
        }
    }

    /*
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1")) 
        {
            TutorialMessgaeObject.SetActive(true);
            TutorialMessgaeObjectfirstone.SetActive(false);
            TutorialMessgaeObjectnextone.SetActive(false);
        }
    }
    */
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            numPlayerCol++;
            TutorialMessgaeObject.SetActive(true);
            TutorialMessgaeObjectfirstone.SetActive(false);
            TutorialMessgaeObjectnextone.SetActive(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            numPlayerCol--;
            if(numPlayerCol == 0)
                TutorialMessgaeObject.SetActive(true);
        }
    }
    */
}
