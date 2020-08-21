using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBreak : MonoBehaviour
{
   
    public GameObject ClosedVersion;
    public GameObject DugUp;
    public ParticleSystem poof;
    public AudioSource sound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickaxe"))
        {
            poof.Play();
            sound.Play();
            ClosedVersion.SetActive(false);
            DugUp.SetActive(true);
        }
    }
}
