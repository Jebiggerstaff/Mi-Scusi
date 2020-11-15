﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    public AudioSource CollideWithWater;
    public AudioSource CollideWithGround;
    public AudioSource CollideWithPlayer;
    public AudioSource PunchedByPlayer;
    public AudioSource AIPunchedByPlayer;
    public AudioSource AIPunchesPlayer;
    public AudioSource KnockedOut;
    //public AudioSource DraggedByPlayer;



    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }
    /*
    private void OnCollisionStay(Collision collision)
    {
        OnTriggerStay(collision.collider);
    }
    */


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<HandContact>() != null)
        {
            tryPlayAudio(PunchedByPlayer);
        }
        else if (other.tag == "Player")
        {
            tryPlayAudio(CollideWithPlayer);
        }
        else if(other.name.Contains("Water") || other.name.Contains("water"))
        {
            tryPlayAudio(CollideWithWater);
        }
        else if(other.tag == "Ground" || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            tryPlayAudio(CollideWithGround);
        }
        else if(GetComponent<NewAIMan>() != null && other.GetComponent<HandContact>() != null)
        {
            var hand = other.GetComponent<HandContact>();
            if((hand.Left && hand.APR_Player.punchingLeft) || (!hand.Left && hand.APR_Player.punchingRight))
            {
                tryPlayAudio(AIPunchedByPlayer);
            }
        }
        else if(GetComponent<AIHandContact>() != null && (other.GetComponent<APRController>() != null || other.GetComponentInParent<APRController>() != null))
        {
            var hand = GetComponent<AIHandContact>();
            if((hand.Left && hand.APR_Player.punchingLeft) || (!hand.Left && hand.APR_Player.punchingRight))
            {
                tryPlayAudio(AIPunchesPlayer);
            }
        }
        //knocked out handled in other scripts
        

    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Ground" || other.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            bool grabbed = false;
            foreach(var hc in FindObjectsOfType<HandContact>())
            {
                if(hc.hasJoint)
                {
                    if(hc.gameObject.GetComponent<FixedJoint>().connectedBody == this.gameObject.GetComponent<Rigidbody>())
                    {
                        grabbed = true;
                    }
                }
            }

           if(grabbed)
            {

                tryPlayAudio(DraggedByPlayer);
            }
        }
    }
    */


    public void tryPlayAudio(AudioSource source)
    {
        if(source != null)
        {
            if(source.isPlaying == false)
            {

                source.Play();

            }
        }
    }
}
