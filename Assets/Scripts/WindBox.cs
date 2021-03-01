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
            if (SceneManager.GetActiveScene().name == "Ship")
                other.gameObject.GetComponent<Rigidbody>().AddForce(-this.transform.up * windPower);
            else
                other.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.up*windPower);
            
        }
    }
}
