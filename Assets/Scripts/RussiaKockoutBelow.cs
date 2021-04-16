using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussiaKockoutBelow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<HostileAI>() != null)
        {
            for(int i = 0; i < 1000; i++)
            {
                other.gameObject.GetComponent<HostileAI>().stun(1);
            }
        }
    }
}
