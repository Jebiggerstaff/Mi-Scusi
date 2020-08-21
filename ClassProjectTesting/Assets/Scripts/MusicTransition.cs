using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransition : MonoBehaviour
{

    public GameObject HideThis;
    public GameObject ShowThis;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideThis.SetActive(false);
            ShowThis.SetActive(true);
        }


    }
}
