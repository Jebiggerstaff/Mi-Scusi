using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
    public int Cactus1Rock2 = 1;

    //if the number is set to 1 then it applies to breakable rocks with pickaxes
    //if the number is set to 2 then it applies to breakable cactuses with the player

    //Don't judge me Alex! I can feel you judging me and you need to stop
    //I am sorry, forgive me...but in my defense the code works...kind of

    public GameObject Unbroken;
    public GameObject Broken;
    public ParticleSystem poof;
    public AudioSource sound;

    private void OnTriggerEnter(Collider other)
    {
        if (Cactus1Rock2 == 1)
        {
            if (other.gameObject.CompareTag("Pickaxe"))
            {
                poof.Play();
                sound.Play();
                Unbroken.SetActive(false);
                Broken.SetActive(true);

            }
        }

        if (Cactus1Rock2 == 2)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                poof.Play();
                sound.Play();
                Unbroken.SetActive(false);
                Broken.SetActive(true);

            }

            // this.transform.GetComponent<ImageEffect>().;
        }


       
        
    }
}
