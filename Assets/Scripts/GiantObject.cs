using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantObject : MonoBehaviour
{
    public int oldLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var ai = collision.gameObject.GetComponent<NewAIMan>();
        if (ai != null)
        {
            for(int i = 0; i < 10; i++)
            {
                ai.stun(5f);
            }
            ai.Explode(collision.GetContact(0).point);
        }
    }
}
