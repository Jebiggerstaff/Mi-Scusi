using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraZoomScript : MonoBehaviour
{
    private MiScusiActions controls;

    public float timeLeft = 15.0f;
    public Animator animator;

    private void Start()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    void Update()
    {

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            animator.enabled = false;
        }

        else if (controls.Player.Jump.triggered)
        {
            if (timeLeft <= 10.0f)
            {
                animator.enabled = false;
            }
            
        }

    }
}
