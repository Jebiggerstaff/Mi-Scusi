using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMap : MonoBehaviour
{
    public APRController controller;
    public GameObject marker;
    public GameObject goal;
    public float movementSpeed;
    public float turnSpeed;
    public GameObject CrashDetector;

    private bool insideMap=true;
    private bool turningLeft, turningRight, goingForward;
    private bool forward, pressed;

    public GameObject LeftTurnButton;
    public GameObject RightTurnButton;
    public GameObject ForwardButton;

    public GameObject lBorder;
    public GameObject rBorder;
    public GameObject bBorder;
    public GameObject tBorder;

    CruiseShipTaskManager cruiseShipTaskManager = new CruiseShipTaskManager();

    public void Start()
    {
        cruiseShipTaskManager = GameObject.Find("TaskUI").GetComponent<CruiseShipTaskManager>();
    }

    void FixedUpdate()
    {

        //Left Turn
        if (controller.RightHand.GetComponent<FixedJoint>() != null)
        {
            if (controller.RightHand.GetComponent<FixedJoint>().connectedBody == LeftTurnButton.gameObject.GetComponent<Rigidbody>())
            {
                turningLeft = true;
            }

        }
        else if(controller.LeftHand.GetComponent<FixedJoint>() != null)
        {
            if (controller.LeftHand.GetComponent<FixedJoint>().connectedBody == LeftTurnButton.gameObject.GetComponent<Rigidbody>())
            {
                turningLeft = true;
            }

        }
        else
        {
            turningLeft = false;
        }
        //Right Turn
        if (controller.RightHand.GetComponent<FixedJoint>() != null)
        {
            if (controller.RightHand.GetComponent<FixedJoint>().connectedBody == RightTurnButton.gameObject.GetComponent<Rigidbody>())
            {
                turningRight = true;
            }

        }
        else if (controller.LeftHand.GetComponent<FixedJoint>() != null)
        {
            if (controller.LeftHand.GetComponent<FixedJoint>().connectedBody == RightTurnButton.gameObject.GetComponent<Rigidbody>())
            {
                turningRight = true;
            }

        }
        else
        {
            turningRight = false;
        }
        //Going Straight
        if (controller.RightHand.GetComponent<FixedJoint>() != null)
        {
            if (controller.RightHand.GetComponent<FixedJoint>().connectedBody == ForwardButton.gameObject.GetComponent<Rigidbody>())
            {
                goingForward = true;
            }

        }
        else if (controller.LeftHand.GetComponent<FixedJoint>() != null)
        {
            if (controller.LeftHand.GetComponent<FixedJoint>().connectedBody == ForwardButton.gameObject.GetComponent<Rigidbody>())
            {
                goingForward = true;
            }

        }
        else
        {
            goingForward = false;
        }


    }

    private void Update()
    {
        if (goingForward || turningLeft || turningRight)
        {
            
        }

        if (goingForward)
        {
            Vector3 proposedPosition = marker.transform.position + transform.forward * movementSpeed;
            if (proposedPosition.z <= tBorder.transform.position.z && proposedPosition.z >= bBorder.transform.position.z && proposedPosition.x <= rBorder.transform.position.x && proposedPosition.x >= lBorder.transform.position.x)
                marker.transform.position += transform.forward*movementSpeed; 
        }

        if (turningLeft)
        {
            transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
        }
        else if (turningRight)
        {
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "NextLevelTarget")
        {
            cruiseShipTaskManager.TaskCompleted("HijackTheShip");
            other.GetComponent<TriggerNextLevel>().loadNextLevel();
        }
    }

}
