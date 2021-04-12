﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLaserTrap : MonoBehaviour
{
    Vector3 origin = new Vector3(245.399994f, 540.700012f, 403.5f);
    Vector3 towards = new Vector3(241.199997f, 540.700012f, 411.399994f);
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = origin - towards;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            FindObjectOfType<APRController>().GotPunched();
            other.attachedRigidbody.AddForce(direction.normalized * 2000f);
            FindObjectOfType<APRController>().Root.GetComponent<Rigidbody>().AddForce(direction.normalized * 2000f);

        }
    }
}
