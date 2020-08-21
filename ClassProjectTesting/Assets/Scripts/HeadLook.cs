using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public GameObject lookatmeCollider;

    
    private void Update()
    {

        if (lookatmeCollider.activeSelf == true)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }
    
}
