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
            if (player == (player | 1 << other.gameObject.layer) && other.gameObject.GetComponent<GrowShrink>() == null && other.GetComponentInChildren<NPC>() == null && other.GetComponent<NPC>() == null && other.GetComponentInParent<NPC>() == null && other.GetComponent<DestroyOverTime>() == null)
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

                    if(other.GetComponent<NewAIMan>() != null)
                    {
                        other.GetComponent<NewAIMan>().maxHP *= 2;
                        other.GetComponent<NewAIMan>().hp *= 2;
                        other.GetComponent<NewAIMan>().agent.height *= 2;
                    }
                    else
                    {
                        if(other.attachedRigidbody.mass >= 16)
                        {

                            other.gameObject.AddComponent<GiantObject>();
                            other.GetComponent<GiantObject>().oldLayer = other.gameObject.layer;
                            other.gameObject.layer = LayerMask.NameToLayer("GiantMeatball");
                        }


                    }
                }

                if (shrink == true && other.attachedRigidbody != null && other.attachedRigidbody.mass >= 1)
                {
                    other.transform.root.localScale = other.transform.root.localScale / 2;
                    //other.transform.root.localScale = new Vector3(1, 1, 1);
                    other.attachedRigidbody.mass = other.attachedRigidbody.mass / 8;

                    if (other.GetComponent<NewAIMan>() != null)
                    {
                        other.GetComponent<NewAIMan>().maxHP /= 2;
                        other.GetComponent<NewAIMan>().hp /= 2;
                        other.GetComponent<NewAIMan>().agent.height /= 2;
                    }
                    else
                    {
                        if(other.attachedRigidbody.mass <= 3)
                        {
                            if(other.GetComponent<GiantObject>() != null)
                            {

                                other.gameObject.layer = other.GetComponent<GiantObject>().oldLayer;
                                Destroy(other.GetComponent<GiantObject>());
                            }

                        }
                    }
                }
            }
        }
    }

    
}
