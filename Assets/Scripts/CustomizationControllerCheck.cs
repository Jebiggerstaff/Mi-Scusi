using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CustomizationControllerCheck : MonoBehaviour
{
    private MiScusiActions controls;

    public GameObject[] Controller;
    public GameObject[] Mouse;

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    void Update()
    {
        if (Gamepad.current == null)
        {
            foreach (var go in Mouse)
                go.SetActive(true);
            foreach (var go in Controller)
                go.SetActive(false);
        }
        else
        {
            foreach (var go in Mouse)
                go.SetActive(false);
            foreach (var go in Controller)
                go.SetActive(true);
        }
    }
}
