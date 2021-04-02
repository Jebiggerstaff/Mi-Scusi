using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
    public Type whatAmI = Type.Rock;

    //if the number is set to 1 then it applies to breakable rocks with pickaxes
    //if the number is set to 2 then it applies to breakable cactuses with the player

    //Don't judge me Alex! I can feel you judging me and you need to stop
    //I am sorry, forgive me...but in my defense the code works...kind of

    public GameObject Unbroken;
    public GameObject Broken;
    public ParticleSystem poof;
    public AudioSource sound;

    

    public enum Type
    {
        Cactus,
        Rock
    }


    private void OnTriggerEnter(Collider other)
    {
        if (whatAmI == Type.Rock)
        {
            if (other.gameObject.CompareTag("Pickaxe"))
            {
                if (gameObject.name == "LockerRoomCrackedWall")
                {
                    FindObjectOfType<RioTaskManager>().TaskCompleted("LockerRoom");
                }
                if(gameObject.name == "StatueWall")
                {
                    FindObjectOfType<RioTaskManager>().TaskCompleted("Booty");

                }
                if(gameObject.name == "RussiaFactoryWall")
                {
                    FindObjectOfType<RussiaTaskManager>().TaskCompleted("BreakIn");
                }
                poof.Play();
                sound.Play();
                Unbroken.SetActive(false);
                Broken.SetActive(true);


            }
        }

        if (whatAmI == Type.Cactus)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                poof.Play();
                sound.Play();
                Unbroken.SetActive(false);
                Broken.SetActive(true);

                FindObjectOfType<DesertCactusEffect>().ActivateCactus();

            }

            // this.transform.GetComponent<ImageEffect>().;
        }


       
        
    }
}
