using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cancelDelayCount = 0;
    }

    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        if (cancelDelayCount > 0)
        {
            cancelDelayCount -= Time.unscaledDeltaTime;
            
        }
        else
        {
            CancelDelay = false;
        }

        if (controls.UI.PauseMenu.triggered)
        {
            if (menu.activeSelf == false && !CancelDelay)
            {

                CancelDelay = true;
                cancelDelayCount = 0.1f;

                //player.SetActive(false);
                menu.SetActive(true);
                menu.GetComponent<SpeedTutorMainMenuSystem.MenuController>().needResumeButton = true;
                menu.GetComponent<SpeedTutorMainMenuSystem.MenuController>().onEnable();

            }
        }

    }


    public GameObject player;
    public GameObject playerother;
    public GameObject playermain;
    public GameObject menu;

    public bool CancelDelay;
    public float cancelDelayCount;



}