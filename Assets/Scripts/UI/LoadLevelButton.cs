using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    private MiScusiActions controls;

    public int index;
    public GameObject[] pics;
    public bool picsSet;
    int currentObj;


    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();

        currentObj = 0;

        
    }
    

    public void TaskOnClick()
    {
        
        if(picsSet)
        {

            currentObj++;
            if (currentObj >= pics.Length)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(index);

            }
            else
            {
                pics[currentObj - 1].SetActive(false);
                pics[currentObj].SetActive(true);
            }

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
