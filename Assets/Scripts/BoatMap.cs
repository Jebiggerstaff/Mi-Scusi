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

    private bool insideMap = true;
    private bool turningLeft, turningRight, goingForward;
    private bool forward, pressed;

    public CruiseBtnHelper LeftTurnButton;
    public CruiseBtnHelper RightTurnButton;
    public CruiseBtnHelper ForwardButton;

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
        if (LeftTurnButton.pressed)
        {
            turningLeft = true;
        }
        else
        {
            turningLeft = false;
        }
        //Right Turn

        if (RightTurnButton.pressed)
        {
            turningRight = true;
        }
        else
        {
            turningRight = false;
        }
        //Going Straight
        if (ForwardButton.pressed)
        {
            goingForward = true;
        }
        else
        {
            goingForward = false;
        }


    }

    private void Update()
    {

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
