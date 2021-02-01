using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip CollideWithWater;
    public AudioClip CollideWithGround;
    public AudioClip CollideWithPlayer;
    public AudioClip PunchedByPlayer;
    public AudioClip AIPunchedByPlayer;
    public AudioClip AIPunchesPlayer;
    public AudioClip KnockedOut;
    //public AudioSource DraggedByPlayer;

    bool collidesWithWater;
    bool collidesWithGround;
    bool collidesWithPlayer;
    bool punchableByPlayer;
    bool aipunchableByPlayer;
    bool aiPunchingPlayer;
    bool knockOutable;


    private void Start()
    {
        collidesWithWater = CollideWithWater != null;
        collidesWithGround = CollideWithGround != null;
        collidesWithPlayer = CollideWithPlayer != null;
        punchableByPlayer = PunchedByPlayer != null;
        aipunchableByPlayer = AIPunchedByPlayer != null;
        aiPunchingPlayer = AIPunchesPlayer != null;
        knockOutable = KnockedOut != null;
    }


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
        if(punchableByPlayer && other.tag == "Player" && other.GetComponent<HandContact>() != null)
        {
            tryPlayAudio(PunchedByPlayer);
        }
        else if (collidesWithPlayer && other.tag == "Player")
        {
            tryPlayAudio(CollideWithPlayer);
        }
        else if(collidesWithWater && (  other.name.Contains("Water") || other.name.Contains("water")))
        {
            tryPlayAudio(CollideWithWater);
        }
        else if(collidesWithGround && (other.tag == "Ground" || other.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            tryPlayAudio(CollideWithGround);
        }
        else if(aipunchableByPlayer && (GetComponent<NewAIMan>() != null && other.GetComponent<HandContact>() != null))
        {
            var hand = other.GetComponent<HandContact>();
            if((hand.Left && hand.APR_Player.punchingLeft) || (!hand.Left && hand.APR_Player.punchingRight))
            {
                tryPlayAudio(AIPunchedByPlayer);
            }
        }
        else if(aiPunchingPlayer && (GetComponent<AIHandContact>() != null && (other.GetComponent<APRController>() != null || other.GetComponentInParent<APRController>() != null)))
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


    public void tryPlayAudio(AudioClip clip)
    {
        if(clip != null)
        {
            if(!(audioSource.clip == clip && audioSource.isPlaying == true))
            {
                audioSource.clip = clip;
                audioSource.Play();

            }
        }
    }
}
