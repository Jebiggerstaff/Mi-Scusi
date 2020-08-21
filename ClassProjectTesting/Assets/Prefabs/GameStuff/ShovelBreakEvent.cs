using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelBreakEvent : MonoBehaviour
{
    public GameObject ClosedVersion;
    public GameObject DugUp;
    public ParticleSystem poof;
    public AudioSource sound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shovel"))
        {
            poof.Play();
            sound.Play();
            ClosedVersion.SetActive(false);
            DugUp.SetActive(true);
        }
    }
}
