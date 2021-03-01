using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    private MiScusiActions controls;

    public int index = 4;

    public GameObject nextpic;

    public int Trans = 1;
    

    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();

        
    }
    

    public void TaskOnClick()
    {
        if (Trans == 1)
        {
            nextpic.SetActive(true);
        }

        else if (Trans == 2)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }

        
    }

    public void Update()
    {
        if (controls.Player.Jump.triggered)
        {
            TaskOnClick();
        }
        
    }
}
