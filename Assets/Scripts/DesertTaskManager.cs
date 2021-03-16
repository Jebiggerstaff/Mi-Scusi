﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class DesertTaskManager : MonoBehaviour
{

    [Header("Tasks")]
    public GameObject[] Tasks;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskUpdatedText;
    public GameObject TaskList;
    public GameObject NextLevel;

    public Text taskList;

    public CosmeticUnlocker CosmeticUnlocker;
    
    [HideInInspector] public int guardsRemoved = 0;
    [HideInInspector] public int AlienParts = 0;
    [HideInInspector] public int ShopThingsMessedWith = 0;

    private bool[] TaskFinished;
    private bool menuOpen;


    string MainTask;

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

        TaskFinished = new bool[Tasks.Length];
        for (int i = 0; i < TaskFinished.Length; i++)
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

        MainTask = "";
        if(TaskFinished[0] == false)
        {
            MainTask = "Remove the guards protecting the Burning Man (" + guardsRemoved.ToString() + "/4)";
        }
        else
        {
            MainTask = "Burn down the Burning Man";
        }



        taskList.text = MainTask + "\n" +
        "Make a mess in the gift shop\n" +
        "Pause the music...forever\n" +
        "Discover what to do with a drunken sailor\n" +
        "Have some fun at the fireworks stand\n" +
        "Win the American Ninja Warrior obstacle race\n" +
        "Find the source of the strange occurences in the cliffs\n";
        
    }


    public void TaskCompleted(string Task)
    {

        switch (Task)
        {
            case "GuardsRemoved":
                if (TaskFinished[0] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                }
                break;
            case "BurnBurningMan":
                if (TaskFinished[0] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                }
                break;
            case "MessUpShop":
                if(TaskFinished[2] == true)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[2].SetActive(true);
                    TaskFinished[2] = true;

                    //TODO: Remove guard
                }
                break;
            case "Music":
                if (TaskFinished[3] == true)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[3].SetActive(true);
                    TaskFinished[3] = true;

                    //TODO: Remove guard
                }
                break;
            case "DrunkenSailor":
                if (TaskFinished[4] == true)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[4].SetActive(true);
                    TaskFinished[4] = true;

                    //TODO: Remove guard
                }
                break;
            case "Fireworks":
                if (TaskFinished[5] == true)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[5].SetActive(true);
                    TaskFinished[5] = true;

                    //TODO: Remove guard
                }
                break;
            case "Race":
                if (TaskFinished[6] == true)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[6].SetActive(true);
                    TaskFinished[6] = true;
                    
                }
                break;
            case "Aliens":
                if (TaskFinished[7] == true)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
                    
                }
                break;
        }
    }
}

