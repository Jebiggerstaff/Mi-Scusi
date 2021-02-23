 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMeatball : MonoBehaviour
{
    public GameObject meatball;

    public void LaunchThatMeatball()
    {
        meatball.SetActive(true);
        meatball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 500, -3000), ForceMode.Impulse);
        Destroy(gameObject);
    }
}
