using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigUp : MonoBehaviour
{
    public KeyCode GrabInput;
    public GameObject ClosedVersion;
    public GameObject DugUp;
    public GameObject Shovel;
    public Collider player;
    public ParticleSystem poof;
    public AudioSource sound;
   // public GameObject destroyedVersion;

    private void OnTriggerStay(Collider player)
    {
       
            if (Input.GetKeyDown(GrabInput))
            {
            if (Shovel.activeSelf == false)
            {
                poof.Play();
                sound.Play();
                ClosedVersion.SetActive(false);
                DugUp.SetActive(true);
               // Instantiate(destroyedVersion, transform.position, transform.rotation);
            }
        }
    }
}
