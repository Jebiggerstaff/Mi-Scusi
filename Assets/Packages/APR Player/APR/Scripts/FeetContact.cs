using System.Collections.Generic;
using UnityEngine;


    //-------------------------------------------------------------
    //--APR Player
    //--Feet Contact
    //
    //--Unity Asset Store - Version 1.0
    //
    //--By The Famous Mouse
    //
    //--Twitter @FamousMouse_Dev
    //--Youtube TheFamouseMouse
    //-------------------------------------------------------------


public class FeetContact : MonoBehaviour
{
	public APRController APR_Player;

    private void Awake()
    {
        allowedWalkLayers.Add("Ground");
        allowedWalkLayers.Add("GiantMeatball");
    }
    private void Start()
    {
        if(SoundSource != null)
            SoundSource.loop = false;
    }

    bool checkAllowedLayers(Collision col)
    {
        foreach (var l in allowedWalkLayers)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(l))
                return true;
        }
        return false;
    }

    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
	{
        if(APR_Player != null)
        {

            if (!APR_Player.isJumping && APR_Player.inAir)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    APR_Player.PlayerLanded();




                }
            }
        }
      
        if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (col.gameObject.GetComponent<WalkingSound>() != null)
            {
                playFootSound(col.gameObject.GetComponent<WalkingSound>().sound);
            }
            else
            {
                playFootSound(defaultSound);
            }
        }
        else if(col.gameObject.GetComponent<WalkingSound>() != null)
        {
            playFootSound(col.gameObject.GetComponent<WalkingSound>().sound);
        }
	}

    void playFootSound(AudioClip clip)
    {
        if(clip != null && SoundSource != null && SoundSource.isPlaying == false)
        {
            SoundSource.clip = clip;
            SoundSource.loop = false;
            SoundSource.Play();
        }
    }


    public AudioSource SoundSource;
    public AudioClip defaultSound;

    List<string> allowedWalkLayers;
}



