using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testingtest : MonoBehaviour
{
   // public GameObject SpawnMe;
    public Collider player;
    public GameObject HideMe;
    public GameObject HideMe2;
    public GameObject Show;
    public GameObject Hide;
    public GameObject ShowNewSpawn;
     float timer = 0f;



    private void OnTriggerEnter(Collider player)
    {
        //Instantiate(SpawnMe, transform.position, transform.rotation);
         //Destroy(gameObject);
        HideMe.SetActive(false);
        HideMe2.SetActive(false);
        Hide.SetActive(false);
        Show.SetActive(true);
        ShowNewSpawn.SetActive(true);

        timer = 2f;

    

    }

    private void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0)
        {
         
        }
    }
}
