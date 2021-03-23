using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedOutForever : MonoBehaviour
{
    public NewAIMan ai;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ai.stunCount > 0)
        {
            ai.stun(1);
        }
        
    }
}
