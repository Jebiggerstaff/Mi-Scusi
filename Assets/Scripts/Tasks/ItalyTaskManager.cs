using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItalyTaskManager : MonoBehaviour
{
    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    [Header("Task Objects")]
    public GameObject MafiaHQ;
    public GameObject[] documents = new GameObject[0];
    public GameObject Fisherman;
    public GameObject Flowers;
    public GameObject[] shopMerchandise = new GameObject[0];
    public GameObject[] fountainMoney = new GameObject[0];



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
            case "HearAboutMafia":
                Tasks[0].SetActive(true);
                break;
            case "BeatUpMafiaMembers":
                Tasks[1].SetActive(true);
                break;
            case "InfiltrateMafiaHQ":
                Tasks[2].SetActive(true);
                break;
            case "Collect5documents":
                Tasks[3].SetActive(true);
                break;
            case "KnockFishermanIntoWater":
                Tasks[4].SetActive(true);
                break;
            case "FlowersToGirl":
                Tasks[5].SetActive(true);
                break;
            case "EatSpaghetti":
                Tasks[6].SetActive(true);
                break;
            case "StealFromShop":
                Tasks[7].SetActive(true);
                break;
            case "GetMoneyFromFountain":
                Tasks[8].SetActive(true);
                break;
            case "KnockoutAngryCustomer":
                Tasks[9].SetActive(true);
                break;
        }
    }
}
