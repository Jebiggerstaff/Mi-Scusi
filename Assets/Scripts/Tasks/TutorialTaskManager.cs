﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskManager : MonoBehaviour
{
    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [HideInInspector] public bool Pickedup = false;
    [HideInInspector] public bool Punched=false;

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
            case "GrabSomething":
                Tasks[0].SetActive(true);
                break;
            case "PunchAGuy":
                Tasks[1].SetActive(true);
                break;
        }

    }
}
