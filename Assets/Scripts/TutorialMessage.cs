using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    public GameObject TutorialMessgaeObject;
    public GameObject TutorialMessgaeObjectfirstone;
    public GameObject TutorialMessgaeObjectnextone;

    // Start is called before the first frame update
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1")) 
        {
            TutorialMessgaeObject.SetActive(true);
            TutorialMessgaeObjectfirstone.SetActive(false);
            TutorialMessgaeObjectnextone.SetActive(false);
        }
    }

    /*
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            TutorialMessgaeObject.SetActive(true);
        }
    }
    */
}
