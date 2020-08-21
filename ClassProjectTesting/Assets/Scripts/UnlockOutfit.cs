using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockOutfit : MonoBehaviour
{
    public GameObject makeHidden1;
    public GameObject makeHidden2;
    public GameObject makeHidden3;
    public GameObject makeHidden4;
    public GameObject makeHidden5;
    public GameObject makeHidden6;
    public GameObject makeHiddenSound;

    private bool hasPlayed = false;

    //  public AudioSource found;
    public Collider player;
    public AudioSource sound;
    // public GameObject collectionbox; 

    private void OnTriggerEnter(Collider player)
    {
       
        makeHidden1.SetActive(false);
        makeHidden2.SetActive(false);
        makeHidden3.SetActive(false);
        makeHidden4.SetActive(false);
        makeHidden5.SetActive(false);
        makeHidden6.SetActive(false);
        if (!hasPlayed)
        {
            sound.Play();
            hasPlayed = true;
        }



        // found.Play();
        // Destroy(collectionbox);
    }
}
