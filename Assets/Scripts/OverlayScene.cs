using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") == 1)
        {
            if (menu.activeSelf == false)
            {
                player.SetActive(false);
                menu.SetActive(true);
                menu.GetComponent<SpeedTutorMainMenuSystem.MenuController>().needResumeButton = true;
                menu.GetComponent<SpeedTutorMainMenuSystem.MenuController>().onEnable();
            }
        }

    }

    public GameObject player;
    public GameObject menu;

}