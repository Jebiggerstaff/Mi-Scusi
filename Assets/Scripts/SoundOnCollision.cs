using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    public AudioSource ThisObjectsSound;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            ThisObjectsSound.Play();
        }

    }
}
