using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : MonoBehaviour
{
    public bool grow = false;
    public bool shrink = false;

    private void OnTriggerEnter(Collider other)
    {
        if (grow == true)
        {
            other.transform.localScale = new Vector3(8, 8, 8);
        }

        if(shrink == true)
        {
            other.transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
}
