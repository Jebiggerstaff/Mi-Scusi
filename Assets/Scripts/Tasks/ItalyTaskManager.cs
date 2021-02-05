﻿using System.Collections;
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

    [HideInInspector] public bool PunchedMafia = false;
    [HideInInspector] public bool PunchedCustomer = false;
    [HideInInspector] public bool AteSpaghetti = false;
    [HideInInspector] public int DocumentsCollected = 0;

    private bool[] TaskFinished = new bool[10];

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

        for (int i = 0; i < 10; i++)
        {
            TaskFinished[i] = false;
        }

    }

    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    public void Update()
    {
        if (controls.UI.TaskMenu.ReadValue<float>() > 0)
        {
            TaskCompleteText.SetActive(false);
            TaskList.SetActive(true);
        }
        if (controls.UI.TaskMenu.ReadValue<float>() == 0)
            TaskList.SetActive(false);
    }

    public void TaskCompleted(string Task)
    {
        RandomAudioMaker.makeAudio(genericCompeltionClip);
        switch (Task)
        {
            case "HearAboutMafia":
                if (TaskFinished[0] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                }
                break;
            case "BeatUpMafiaMembers":
                if (TaskFinished[1] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                }
                break;
            case "InfiltrateMafiaHQ":
                if (TaskFinished[2] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[2].SetActive(true);
                    TaskFinished[2] = true;
                }
                break;
            case "Collect5documents":
                if (TaskFinished[3] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[3].SetActive(true);
                    TaskFinished[3] = true;
                }
                break;
            case "KnockFishermanIntoWater":
                if (TaskFinished[4] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[4].SetActive(true);
                    TaskFinished[4] = true;
                }
                break;
            case "FlowersToGirl":
                if (TaskFinished[5] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[5].SetActive(true);
                    TaskFinished[5] = true;
                }
                break;
            case "EatSpaghetti":
                if (TaskFinished[6] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[6].SetActive(true);
                    TaskFinished[6] = true;
                }
                break;
            case "StealFromShop":
                if (TaskFinished[7] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
                }
                break;
            case "GetMoneyFromFountain":
                if (TaskFinished[8] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[8].SetActive(true);
                    TaskFinished[8] = true;
                }
                break;
            case "KnockoutAngryCustomer":
                if (TaskFinished[9] == false)
                {
                    TaskCompleteText.SetActive(true);
                    Tasks[9].SetActive(true);
                    TaskFinished[9] = true;
                }
                break;
        }
    }
}
