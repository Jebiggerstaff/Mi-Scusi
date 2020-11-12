using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    public float RespawnHeight;

    // Update is called once per frame
    void Update()
    {
        if(Player.transform.position.y < RespawnHeight)
        {
            Player.transform.position = SpawnPoint.transform.position;
        }
    }
}
