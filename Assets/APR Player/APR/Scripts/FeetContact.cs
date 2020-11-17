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

    private void Start()
    {
        SoundSource.loop = false;
    }

    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
	{
        if(!APR_Player.isJumping && APR_Player.inAir)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                APR_Player.PlayerLanded();

                


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
}



