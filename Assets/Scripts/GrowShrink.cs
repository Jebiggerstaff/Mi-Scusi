using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : MonoBehaviour
{
    public bool grow = false;
    public bool shrink = false;
    public LayerMask player;

    private void OnTriggerEnter(Collider other)
    {
        if (grow == true && other.attachedRigidbody.mass <= 60)
        {
            other.transform.root.localScale = other.transform.root.localScale * 2;
            //other.transform.root.localScale = new Vector3(8, 8, 8);
            other.attachedRigidbody.mass = other.attachedRigidbody.mass * 8;
        }

        if(shrink == true && other.attachedRigidbody.mass >= 1)
        {
            other.transform.root.localScale = other.transform.root.localScale / 2;
            //other.transform.root.localScale = new Vector3(1, 1, 1);
            other.attachedRigidbody.mass = other.attachedRigidbody.mass / 8;
        }
    }
}
