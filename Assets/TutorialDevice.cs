using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TutorialDevice : MonoBehaviour
{
    private MiScusiActions controls;

    

    public GameObject TutorialKeyboard;
    public GameObject TutorialController;

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    void Update()
    {
        if (Gamepad.current==null)
        {
            TutorialController.SetActive(true);
        }
        else
        {
            TutorialKeyboard.SetActive(true);
        }
    }
}
