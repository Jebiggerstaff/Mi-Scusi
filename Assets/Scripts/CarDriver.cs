using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public GameObject[] RoadPoints = new GameObject[0];
    public float carSpeed = 0f;
    public float CrashingForce=10f;

    private Vector3 currentLocation;
    private int targetPoint=0;

    private void Start()
    {
        transform.position = RoadPoints[0].transform.position;
    }

    void Update()
    {
        currentLocation = this.gameObject.transform.position;

        transform.LookAt(RoadPoints[targetPoint].transform);

        if (currentLocation != RoadPoints[targetPoint].transform.position)
            transform.position = Vector3.MoveTowards(transform.position, RoadPoints[targetPoint].transform.position, carSpeed);

        else
        {
            if (targetPoint + 1 != RoadPoints.Length)
                targetPoint += 1;
            else
                targetPoint = 0;
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "APR_Root")
        {
            Debug.Log("Bonk");
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * CrashingForce);
            
        }
    }
}
