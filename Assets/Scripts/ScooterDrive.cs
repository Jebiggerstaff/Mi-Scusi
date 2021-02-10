using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterDrive : MonoBehaviour
{
    public float speed;
    public GameObject frontTire, backTire;

    public bool driving = false, Grounded = true;
    RaycastHit hit;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("FinalPlayer");
    }

    private void Update()
    {

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 3f) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Grounded = true;
        else
            Grounded = false;


        if (driving&&Grounded)
        {
            this.GetComponent<Rigidbody>().AddForce(-this.transform.forward*speed);
            player.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(-this.transform.forward * speed);

            frontTire.transform.Rotate(Vector3.left * Time.deltaTime * 4f, Space.Self);
            backTire.transform.Rotate(Vector3.left*Time.deltaTime*4f,Space.Self);

            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HandContact>() != null)
        {
            driving = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<HandContact>() != null)
        {
            driving = false;
        }
    }

}
