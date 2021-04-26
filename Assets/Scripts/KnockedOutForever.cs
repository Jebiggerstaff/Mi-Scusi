using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedOutForever : MonoBehaviour
{
    public NewAIMan ai;
    public SoundOnCollision sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<SoundOnCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ai.stunCount > 0)
        {
            ai.stun(1);

            if (sound != null)
                Destroy(sound);
            

            if(ai.anim.speed > 0)
            {
                ai.anim.speed = Mathf.Clamp(ai.anim.speed - Time.deltaTime, 0, 2);
                Debug.Log(ai.anim.speed);
            }

            if (GetComponent<NPC>() != null)
                Destroy(GetComponent<NPC>());
                

            if(GetComponentsInChildren<Canvas>().Length > 0)
            {
                foreach (var c in GetComponentsInChildren<Canvas>())
                    Destroy(c.gameObject);
            }
        }
        
    }
}
