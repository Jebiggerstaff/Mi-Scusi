using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    Rigidbody rb;
    public KeyCode GrabInput, UnGrabInput;
    public GameObject MyGrabObj;
    public bool IsGrab = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MyGrabObj != null)
        {
            if (Input.GetKey(GrabInput))
            {
                if (!IsGrab)
                {
                    FixedJoint Fj = MyGrabObj.AddComponent<FixedJoint>();
                    Fj.connectedBody = rb;
                    Fj.breakForce = 8000;
                    IsGrab = true;
                }
            }
            else if (Input.GetKey(UnGrabInput))
            {
                if (MyGrabObj.CompareTag("Item"))
                {
                    Destroy(MyGrabObj.GetComponent<Joint>());
                }
                MyGrabObj = null;
                IsGrab = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            MyGrabObj = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            MyGrabObj = other.gameObject;
        }
    }
}
