using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTravel : MonoBehaviour
{
    public APRController player;
    bool pull;



    private void Awake()
    {   pull = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        if(pull)
        {
            player.Root.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(player.Root.GetComponent<Rigidbody>().position, transform.position, .5f));
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<FeetContact>() != null)
        {
            pull = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.GetComponent<FeetContact>() != null)
        {
            pull = false;
        }
    }


}
