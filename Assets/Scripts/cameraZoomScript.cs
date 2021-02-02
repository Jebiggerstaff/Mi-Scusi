using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraZoomScript : MonoBehaviour
{
    private MiScusiActions controls;

    public float timeLeft = 15.0f;
    public Animator animator;

    private void Awake()
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
            animator.enabled = false;
        }

    }
}
