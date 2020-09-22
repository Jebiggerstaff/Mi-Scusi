using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMap : MonoBehaviour
{
    public GameObject steeringWheel;
    public float rotSpeed;

    void FixedUpdate()
    {
        Debug.Log(steeringWheel.transform.localRotation.eulerAngles.x);

        if (steeringWheel.transform.rotation.x > -85)
        {
            this.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.eulerAngles.y + rotSpeed, transform.rotation.z,transform.rotation.w);
        }
        else if (steeringWheel.transform.rotation.x < -95)
        {
            this.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.eulerAngles.y - rotSpeed, transform.rotation.z, transform.rotation.w);
        }


    }
}
