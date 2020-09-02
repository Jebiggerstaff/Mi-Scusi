using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;
    public Vector3 CamPos;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    RaycastHit hit;

    private void Awake()
    {
        CamPos = Offset;
    }

    private void Update()
    {
        if (Physics.Linecast(transform.position, Player.position,out hit))
        {
            if (hit.transform.tag == "Buildings")
            {
                Camera.main.transform.position += Camera.main.transform.forward * 1f; ;
            }
            else
            {
                
            }
        }

    }

    void LateUpdate()
    {
        if (Player != null)
            transform.position = Vector3.SmoothDamp(transform.position, Player.position+Offset, ref velocity, smoothTime);
        
    }

}
