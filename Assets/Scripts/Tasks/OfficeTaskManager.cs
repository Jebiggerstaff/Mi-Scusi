using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTaskManager : MonoBehaviour
{
    //Public Variables
    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    [Header("Task Objects")]

    public GameObject WaterCoolerJug;
    public GameObject Coffee;

    public GameObject PaperStack1;
    public GameObject PaperStack2;
    public GameObject PaperStack3;

    public GameObject PhoneNumber;


    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

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
            case "RefilWater":
                Tasks[0].SetActive(true);
                PaperStack1.SetActive(true);
                break;
            case "HelpCoworker":
                Tasks[1].SetActive(true);
                PaperStack2.SetActive(true);
                break;
            case "GetBossCoffee":
                Tasks[2].SetActive(true);
                PaperStack3.SetActive(true);
                break;
            case "CopyButt":
                Tasks[3].SetActive(true);
                break;
        }

    }
}
