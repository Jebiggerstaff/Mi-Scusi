using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    public GameObject TutorialMessgaeObject;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            TutorialMessgaeObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            TutorialMessgaeObject.SetActive(false);
        }
    }

}
