using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float icyness;


    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null && (controls.Player.MoveX.ReadValue<float>()==0 || controls.Player.MoveY.ReadValue<float>() == 0))
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3 (Random.Range(-100, 100) + other.GetComponent<Rigidbody>().velocity.x*icyness,0,
                Random.Range(-100, 100) + other.GetComponent<Rigidbody>().velocity.z * icyness));
        }
    }
    

    /*
     * Slow Down
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3 (-other.GetComponent<Rigidbody>().velocity.x*icyness,0,-other.GetComponent<Rigidbody>().velocity.z * icyness));
        }
    }
    */
    /*
     * Speed UP
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3 (other.GetComponent<Rigidbody>().velocity.x*icyness,0,other.GetComponent<Rigidbody>().velocity.z * icyness));
        }
    }
    */

}
