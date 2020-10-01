using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDevice : MonoBehaviour
{

    public APRController con;
    public GameObject TutorialKeyboard;
    public GameObject TutorialController;

    void Update()
    {
        if (con.usingController)
        {
            TutorialController.SetActive(true);
        }
        else
        {
            TutorialKeyboard.SetActive(true);
        }
    }
}
