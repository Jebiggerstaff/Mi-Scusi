using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpinOnDrag : MonoBehaviour
{
    bool rotating = false;
   float direction = 1;
    MiScusiActions controls;

    public GameObject[] rotateBtns;

    // Start is called before the first frame update
    void Start()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current != null)
        {
            if (controls.UI.RotateChar.ReadValue<float>() != 0)
            {
                direction = controls.UI.RotateChar.ReadValue<float>();
                rotating = true;
            }
            else
            {
                rotating = false;
            }

            foreach(var go in rotateBtns)
            {
                go.SetActive(false);
            }

        }
        else
        {

            foreach (var go in rotateBtns)
            {
                go.SetActive(true);
            }
        }
        if(rotating)
            RotateOnFrame();
    }

    public void RotateOnFrame()
    {
        transform.Rotate(new Vector3(0, direction * 135 * Time.unscaledDeltaTime, 0));
        
    }
    public void StartRotating(float dir)
    {
        direction = dir;
        rotating = true;
    }
    public void EndRotating()
    {
        rotating = false;
    }

    
    
}
