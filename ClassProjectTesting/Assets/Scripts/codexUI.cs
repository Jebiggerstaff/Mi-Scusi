using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class codexUI : MonoBehaviour
{
    public GameObject Panel;

    public GameObject Hide1;
    public GameObject Hide2;
    public GameObject Hide3;
    public GameObject Hide4;
    public GameObject Hide5;
    public GameObject Hide6;
    public GameObject Hide7;
    public GameObject Hide8;
    public GameObject Hide9;

    public AudioSource clickSound;

    public void OpenPanel()
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
            Hide1.SetActive(false);
            Hide2.SetActive(false);
            Hide3.SetActive(false);
            Hide4.SetActive(false);
            Hide5.SetActive(false);
            Hide6.SetActive(false);
            Hide7.SetActive(false);
            Hide8.SetActive(false);
            Hide9.SetActive(false);
            clickSound.Play();
        }
    }
}
