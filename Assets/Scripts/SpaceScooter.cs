using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceScooter : MonoBehaviour
{
    public float speed;

    public bool driving = false;
    RaycastHit hit;
    private GameObject player;

    [HideInInspector]
    public int driveCount = 0;

    private void Start()
    {
        player = GameObject.Find("FinalPlayer");
    }

    private void Update()
    {

        driving = driveCount >= 2;


        if (driving)
        {
            Vector3 force = transform.forward * speed;
            force.y /= 8;

            this.GetComponent<Rigidbody>().AddForce(force);
            player.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(force);
            

           this.GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }



}
