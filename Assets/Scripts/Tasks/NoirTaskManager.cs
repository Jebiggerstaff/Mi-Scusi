using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoirTaskManager : MonoBehaviour
{
    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Murder GameObjects")]
    public GameObject[] Suspects = new GameObject[0];
    public GameObject MurderWeapon;

    void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
    }

    public void Update()
    {
        if (Input.GetButtonDown("TaskMenu"))
        {
            TaskCompleteText.SetActive(false);
            TaskList.SetActive(true);
        }
        if (Input.GetButtonUp("TaskMenu"))
            TaskList.SetActive(false);
    }

    public void TaskCompleted(string Task)
    {
        TaskCompleteText.SetActive(true);
        switch (Task)
        {
            case "MurderWeapon":
                Tasks[0].SetActive(true);
                break;
            case "MurderSuspect":
                Tasks[1].SetActive(true);
                break;
            case "WantedPoster":
                Tasks[2].SetActive(true);
                break;
            case "Sevens":
                Tasks[3].SetActive(true);
                break;
        }

    }

}
