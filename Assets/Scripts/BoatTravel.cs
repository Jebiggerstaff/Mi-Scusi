using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTravel : MonoBehaviour
{
    List<GameObject> collisions;
    public GameObject player;
    Vector3 prevPos;


    private void Awake()
    {
        collisions = new List<GameObject>();
        prevPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(collisions.Count >= 1)
        {
            Vector3 changeInPos = transform.position - prevPos;
            player.transform.position += changeInPos;
        }
        prevPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            collisions.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            collisions.Remove(collision.gameObject);
        }
    }
    
}
