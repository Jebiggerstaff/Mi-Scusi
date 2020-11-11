using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTurnOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitGet());
    }

    // Update is called once per frame
    void Update()
    {
        if(allRigidbodies != null)
        {
            foreach(var rb in allRigidbodies)
            {
                if(Vector3.Distance(player.position, rb.transform.position) > distance)
                {
                    rb.detectCollisions = false;
                    if(rb.GetComponent<NewAIMan>() == null)
                    {
                        rb.isKinematic = true;
                    }
                }
                else
                {
                    rb.detectCollisions = true;
                    if (rb.GetComponent<NewAIMan>() == null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
        }
    }


    IEnumerator waitGet()
    {
        yield return new WaitForSeconds(0.5f);
        allRigidbodies = new List<Rigidbody>(FindObjectsOfType<Rigidbody>());

    }

    List<Rigidbody> allRigidbodies = null;
    public float distance = 150f;
    public Transform player;
}
