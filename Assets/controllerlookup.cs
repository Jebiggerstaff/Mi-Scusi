using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class controllerlookup : MonoBehaviour
{
    private MiScusiActions controls;

    public GameObject spacebar;
    public GameObject A;
    public GameObject X;

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    void Update()
    {
        if (Gamepad.current == null)
        {
            spacebar.SetActive(true);
        }
        else if(Gamepad.current.name== "DualShock4GamepadHID")
        {
            X.SetActive(true);
        }
        else if(Gamepad.current.name == "XBox One Controller")
        {
            A.SetActive(true);
        }
        else
        {
            A.SetActive(true);
        }
    }
}
