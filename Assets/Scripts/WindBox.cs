﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindBox : MonoBehaviour
{
    bool facingUp = true;

    public float windPower = 0f;
    float soundCD = 0;

    Vector3 direction;



    private void Update()
    {
        if (soundCD > 0)
            soundCD -= Time.deltaTime;
        if(facingUp)
        {
            direction = transform.up;
        }
        else
        {
            direction = -transform.up;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            if (other.gameObject.GetComponent<NoPushScript>() == null)
            {
                if (other.gameObject.GetComponent<NewAIMan>() != null)
                {
                    other.gameObject.GetComponent<NewAIMan>().stun(0.25f);
                }
                if(soundCD <= 0)
                {


                }
                other.gameObject.GetComponent<Rigidbody>().AddForce(direction * windPower);
            }
        }
    }

    public void SetDirection(bool up)
    {
        facingUp = up;
    }


}
