using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject Player;
    public GameObject SpawnPoint;
    public float RespawnHeight;

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in objects) {


            if (obj != null &&  obj.transform.position.y < RespawnHeight)
            {
                obj.transform.position = SpawnPoint.transform.position;
            }
                

        } 

        if (Player.transform.position.y < RespawnHeight)
        {
            Player.transform.position = SpawnPoint.transform.position;
        }
    }
}
