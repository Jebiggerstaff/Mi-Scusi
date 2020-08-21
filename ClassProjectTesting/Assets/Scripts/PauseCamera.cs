using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCamera : MonoBehaviour
{

    public GameObject pausecamera;
   // public GameObject camera1;
   // public GameObject camera2;
    public GameObject pausescreen;
    public GameObject pausepanel1;
    public GameObject pausepanel2;
    public GameObject AttirePanel;
    public GameObject HatPanel;
    public GameObject bodyPanel;
    public GameObject armPanel;
    public GameObject legPanel;
    public GameObject armPanel2;
    public GameObject legPanel2;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab) && pausecamera.activeSelf == false)
            if (pausescreen.activeSelf == true)
            {
            pausecamera.SetActive(true);
          //  camera1.SetActive(false);
         
        }

       // else if (Input.GetKeyDown(KeyCode.Tab) && pausecamera.activeSelf == true)
             else if (pausescreen.activeSelf == false && pausepanel1.activeSelf == false && pausepanel2.activeSelf == false && AttirePanel.activeSelf == false && HatPanel.activeSelf == false
            && armPanel.activeSelf == false && legPanel.activeSelf == false && bodyPanel.activeSelf == false && armPanel2.activeSelf == false && legPanel2.activeSelf == false)
        {
            pausecamera.SetActive(false);
          //  camera1.SetActive(true);
        
        }

            if (Input.GetKeyDown(KeyCode.Tab))
        {
            AttirePanel.SetActive(false);
            HatPanel.SetActive(false);
            bodyPanel.SetActive(false);
            legPanel.SetActive(false);
            armPanel.SetActive(false);
            legPanel2.SetActive(false);
            armPanel2.SetActive(false);
        }
    }
}
