using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public float bounciness = .9f;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            Vector3 Incoming = other.gameObject.GetComponent<Rigidbody>().velocity;
            if (Incoming.y < 15f/bounciness)
            {
                Debug.Log(other.gameObject.name + " Bounced");
                if(other.gameObject.name=="APR_RightFoot"||other.gameObject.name=="APR_LeftFoot")
                    GameObject.Find("APR_Root").GetComponent<Rigidbody>().AddForce(this.transform.up * bounciness * 10000);
            }
        }
    }

}
