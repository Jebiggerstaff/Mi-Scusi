using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndUnhide : MonoBehaviour
{
    public GameObject HideThis;
    public GameObject ShowThis;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Activator"))
        {
            HideThis.SetActive(false);
            ShowThis.SetActive(true);
        }
            
    
    }
}
