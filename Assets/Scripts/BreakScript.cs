using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
    public GameObject Unbroken;
    public GameObject Broken;
    public ParticleSystem poof;
    public AudioSource sound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickaxe"))
        {
            poof.Play();
            sound.Play();
            Unbroken.SetActive(false);
            Broken.SetActive(true);
        }
    }
}
