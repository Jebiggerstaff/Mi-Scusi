using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolNoodle : MonoBehaviour
{
    [HideInInspector]
    public bool isGrabbed;
    [HideInInspector]
    public int handCount;
    public AudioClip bonk;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrabbed = handCount > 0;
    }
    

    public void CollideThings(Collision collision)
    {
        var ai = collision.gameObject.GetComponent<NewAIMan>();
        if (ai != null)
        {
            if (isGrabbed)
            {
                RandomAudioMaker.makeAudio(bonk);
                ai.Explode(transform.position);
                ai.Explode(transform.position);
                ai.stun(5f);
                ai.stun(5f);
                ai.stun(5f);
            }
        }
        else
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollideThings(collision);
    }
}
