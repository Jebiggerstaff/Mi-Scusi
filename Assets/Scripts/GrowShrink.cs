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
      

        if (other.gameObject != gameObject)
        {
            if (player == (player | 1 << other.gameObject.layer) && other.gameObject.GetComponent<GrowShrink>() == null && other.GetComponentInChildren<NPC>() == null && other.GetComponent<NPC>() == null && other.GetComponentInParent<NPC>() == null)
            {
                if (grow == true && other.attachedRigidbody != null && other.attachedRigidbody.mass <= 60)
                {
                    other.transform.root.localScale = other.transform.root.localScale * 2;
                    //other.transform.root.localScale = new Vector3(8, 8, 8);
                    other.attachedRigidbody.mass = other.attachedRigidbody.mass * 8;

                    if(other.gameObject.name.Contains("WantedPoster") && other.attachedRigidbody.mass > 60)
                    {
                        FindObjectOfType<RussiaTaskManager>().TaskCompleted("Portrait");
                    }
                }

                if (shrink == true && other.attachedRigidbody != null && other.attachedRigidbody.mass >= 1)
                {
                    other.transform.root.localScale = other.transform.root.localScale / 2;
                    //other.transform.root.localScale = new Vector3(1, 1, 1);
                    other.attachedRigidbody.mass = other.attachedRigidbody.mass / 8;
                }
            }
        }
    }

    
}
