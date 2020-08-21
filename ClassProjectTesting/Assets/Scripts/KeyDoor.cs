using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameObject ClosedVersion;
    public GameObject OpenVersion;
    public GameObject CorrelatedKey;
    public Collider player;

    private void OnTriggerEnter(Collider player)
    {
        if(CorrelatedKey.activeSelf == false)
        {
            ClosedVersion.SetActive(false);
            OpenVersion.SetActive(true);
        }
        //Instantiate(ClosedVersion, transform.position, transform.rotation);
        //Destroy(gameObject);
    }
}
