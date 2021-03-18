using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoirTaskManager : MonoBehaviour
{

    public GameObject confetti;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    public CosmeticUnlocker CosmeticUnlocker;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Murder GameObjects")]
    public GameObject[] Suspects = new GameObject[0];
    public GameObject MurderWeapon;
    private bool menuOpen;



    void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
    }

    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
        
    }

    public void Update()
    {

        if (CosmeticUnlocker == null)
            CosmeticUnlocker = FindObjectOfType<OverlayScene>().menu.GetComponent<CosmeticUnlocker>();

        if (controls.UI.TaskMenu.triggered && menuOpen == false)
        {
            TaskCompleteText.SetActive(false);
            TaskList.SetActive(true);
            menuOpen = true;
        }
        else if (controls.UI.TaskMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }
    }

    public void TaskCompleted(string Task)
    {
        TaskCompleteText.SetActive(true);
        switch (Task)
        {
            case "MurderWeapon":
                Tasks[1].SetActive(true);
                break;
            case "MurderSuspect":
                Tasks[0].SetActive(true);
                break;
            case "WantedPoster":
                Tasks[2].SetActive(true);
                break;
            case "Sevens":
                Tasks[3].SetActive(true);
                break;
        }

    }

    IEnumerator confettistuff()
    {
        confetti.SetActive(true);
        yield return new WaitForSeconds(2);
        confetti.SetActive(false);
    }

}
