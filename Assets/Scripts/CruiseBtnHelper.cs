using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiseBtnHelper : MonoBehaviour
{
    int count = 0;
    public bool pressed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pressed = count > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            count++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            count--;
        }
    }
}
