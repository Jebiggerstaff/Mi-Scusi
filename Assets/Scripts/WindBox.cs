using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindBox : MonoBehaviour
{

    public float windPower = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            if (other.gameObject.GetComponent<NewAIMan>() != null)
            {
                other.gameObject.GetComponent<NewAIMan>().stun(0.25f);
            }
            other.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.up * windPower);



        }
    }
}
