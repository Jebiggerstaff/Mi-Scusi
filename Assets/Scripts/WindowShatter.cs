using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowShatter : MonoBehaviour
{

    public GameObject destroyedVersion;

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CanBeGrabbed")
        {
           
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }
}
