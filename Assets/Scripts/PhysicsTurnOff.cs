using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTurnOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitialTurnOff();
        StartCoroutine(waitGet());
    }

    // Update is called once per frame
    void Update()
    {
        //TurnOffCode();
    }

    void TurnOffCode()
    {
        if (allRigidbodies != null && allRigidbodies.Count > 0)
        {

            //Debug.Log("Total Rigidbodies: " + allRigidbodies.Count);
            for (int i = 0; i < 20; i++)
            {
                var rb = allRigidbodies[currentCount];
                if (rb == null || rb.GetComponent<NewAIMan>() != null)
                {
                    allRigidbodies.Remove(rb);
                }
                else
                {
                    if (Vector3.Distance(player.position, rb.transform.position) > distance)
                    {
                        //rb.detectCollisions = false;
                        if (rb.isKinematic == false)
                        {
                            rb.isKinematic = true;
                            if (rb.GetComponent<SoundOnCollision>() != null)
                            {
                                rb.GetComponent<SoundOnCollision>().enabled = false;
                            }
                            Debug.Log("Turning Off " + rb.gameObject.name);
                        }
                    }
                    else
                    {
                        if (rb.isKinematic == true)
                        {
                            rb.isKinematic = false;
                            if (rb.GetComponent<SoundOnCollision>() != null)
                            {
                                rb.GetComponent<SoundOnCollision>().enabled = true;
                            }
                            Debug.Log("Turning On " + rb.gameObject.name);
                        }
                    }
                    currentCount++;
                    if (currentCount >= allRigidbodies.Count)
                    {
                        currentCount = 0;
                    }
                }
            }
        }
    }

    public void InitialTurnOff()
    {

        allRigidbodies = new List<Rigidbody>(FindObjectsOfType<Rigidbody>()); Rigidbody rb;
        while (currentCount < allRigidbodies.Count)
        {
            rb = allRigidbodies[currentCount];
            if (rb == null || rb.GetComponent<NewAIMan>() != null)
            {
                allRigidbodies.Remove(rb);
            }
            else
            {
                currentCount++;
            }
        }
        currentCount = 0;

        foreach(var x in allRigidbodies)
        {
            rb = x;
            if (Vector3.Distance(player.position, rb.transform.position) > distance)
            {
                //rb.detectCollisions = false;
                if (rb.isKinematic == false)
                {
                    rb.isKinematic = true;
                    if (rb.GetComponent<SoundOnCollision>() != null)
                    {
                        rb.GetComponent<SoundOnCollision>().enabled = false;
                    }
                    Debug.Log("Turning Off " + rb.gameObject.name);
                }
            }
            else
            {
                if (rb.isKinematic == true)
                {
                    rb.isKinematic = false;
                    if (rb.GetComponent<SoundOnCollision>() != null)
                    {
                        rb.GetComponent<SoundOnCollision>().enabled = true;
                    }
                    Debug.Log("Turning On " + rb.gameObject.name);
                }
            }
        }
    }


    IEnumerator waitGet()
    {
        yield return new WaitForSeconds(0.5f);
        if(allRigidbodies != null)
        {
            allRigidbodies.Clear();
        }
        allRigidbodies = new List<Rigidbody>(FindObjectsOfType<Rigidbody>());
        
        currentCount = 0;
        Rigidbody rb;
        while (currentCount < allRigidbodies.Count)
        {
            rb = allRigidbodies[currentCount];
            if (rb == null || rb.GetComponent<NewAIMan>() != null)
            {
                allRigidbodies.Remove(rb);
            }
            else
            {
                currentCount++;
            }
        }
        currentCount = 0;

    }

    List<Rigidbody> allRigidbodies = null;
    public float distance = 150f;
    public Transform player;
    public int currentCount = 0;
}
