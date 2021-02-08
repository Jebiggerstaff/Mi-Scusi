using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrip : MonoBehaviour
{
    private float fixedDeltaTime;
    private MiScusiActions controls;

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();

        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (controls.Player.RightPunch.triggered)
        {
            Debug.Log("Scaled Down");
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.7f;
            else
                Time.timeScale = 1.0f;
        }
        if (controls.Player.LeftPunch.triggered)
        {
            Debug.Log("Scaled Up");
            if (Time.timeScale == 1.0f)
                Time.timeScale = 1.6f;
            else
                Time.timeScale = 1.0f;
        }
    }
}
