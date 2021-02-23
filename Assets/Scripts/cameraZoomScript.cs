using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraZoomScript : MonoBehaviour
{
    private MiScusiActions controls;

    public float timeLeft = 15.0f;

    float badTime;

    public Animator animator;
    public GameObject fadeCube;

    private void Start()
    {
        controls = new MiScusiActions();
        controls.Enable();
        badTime = timeLeft - 3.0f;
    }

    void Update()
    {
        
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            animator.enabled = false;
            fadeCube.SetActive(false);
        }

        else if (controls.Player.Jump.triggered)
        {
            if (timeLeft <= badTime)
            {
                animator.enabled = false;
            fadeCube.SetActive(false);
            }
            
        }

    }
}
