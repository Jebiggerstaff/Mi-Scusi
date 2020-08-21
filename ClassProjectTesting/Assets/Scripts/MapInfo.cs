using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public GameObject Text;
    public Collider player;



    private void OnTriggerEnter(Collider player)
    {
            Text.SetActive(true);
    }

    private void OnTriggerExit(Collider player)
    {
        Text.SetActive(false);
    }
}
