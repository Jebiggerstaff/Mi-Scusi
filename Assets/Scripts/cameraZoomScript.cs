using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoomScript : MonoBehaviour
{
    public float timeLeft = 15.0f;
    public Animator animator;
  

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            animator.enabled = false;
        }
    }
}
