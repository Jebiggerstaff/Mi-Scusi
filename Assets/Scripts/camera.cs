using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (Player != null)
            transform.position = Vector3.SmoothDamp(transform.position, Player.position+Offset, ref velocity, smoothTime);
        
    }
}
