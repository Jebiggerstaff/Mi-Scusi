using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioTaskCollider : MonoBehaviour
{
    public RioTaskManager RioTaskManager;
    public NPC RelevantNPC;

    private void Awake()
    {
        RioTaskManager = FindObjectOfType<RioTaskManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(name == "Goalmarker" && other.gameObject.name == "SoccerBall")
        {
            RioTaskManager.TaskCompleted("Goal");
        }
        if(name == "GetawaySpot" && other.gameObject.name == "Trophy")
        {
            RioTaskManager.TaskCompleted("Getaway");
        }
        if(name == "CheckBallFall" && other.gameObject.name == "GiantBall")
        {
            RioTaskManager.TaskCompleted("Ball");
        }


        if(name == "Shop1" && other.gameObject.name == "Fan")
        {
            RioTaskManager.Scooter.SetActive(true);
            Destroy(other.gameObject);
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(gameObject);
            

        }

        if(name == "WaterQuad" && other.gameObject.name.Contains("Mafia"))
        {
            RioTaskManager.TaskCompleted("CrimeLord");

            for(int i = 0; i < 1000; i++)
                other.GetComponent<NewAIMan>().stun(0.5f);

            Destroy(other.GetComponent<NPC>());
            foreach(var c in other.GetComponentsInChildren<Canvas>())
            {
                Destroy(c.gameObject);
            }

            //UNLOCK COSTUMES
        }


        if (name == "Shop2" && other.gameObject.name == "Scooter")
        {
            RioTaskManager.Sword.SetActive(true);
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (name == "Shop3" && other.gameObject.name == "ToySword")
        {
            RioTaskManager.Bike.SetActive(true);
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(other.gameObject);
            Destroy(gameObject);

        }
        if (name == "Shop4" && other.gameObject.name == "TaskBike")
        {
            RioTaskManager.Coin.SetActive(true);
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (name == "Shop5" && other.gameObject.name == "TaskCoin")
        {
            RioTaskManager.UFO.SetActive(true);
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (name == "Shop6" && other.gameObject.name == "TaskUFO")
        {
            RioTaskManager.miniTrophy.SetActive(true);
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (name == "Shop7" && other.gameObject.name == "MiniTrophy")
        {
            RioTaskManager.TaskCompleted("Shopping");
            RelevantNPC.sentences = new string[1];
            RelevantNPC.sentences[0] = "Thanks!  I got what I want.";
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
