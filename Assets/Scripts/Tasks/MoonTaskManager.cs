﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonTaskManager : MonoBehaviour
{
    public GameObject confetti;

    [Header("Tasks")]
    public GameObject[] Tasks;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskUpdatedText;
    public GameObject TaskList;
    public GameObject NextLevel;
    public Text taskList;
    [Space]
    [HideInInspector] public int PowerCellsRemoved;
    public GameObject Door;
    [Space]
    public GameObject b1;
    public GameObject b2;
    public GameObject b3;
    public GameObject b4;
    public GameObject b5;
    public GameObject b6;
    public Material CompletedMat;
    [Space]


    public CosmeticUnlocker CosmeticUnlocker;

    [HideInInspector] public bool[] TaskFinished;
    private bool menuOpen=true;


    string MainTask;

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;
    public AudioClip PowerClip;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = 1.2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

        TaskFinished = new bool[Tasks.Length];
        for (int i = 0; i < TaskFinished.Length; i++)
        {
            TaskFinished[i] = false;
        }

        menuOpen = true;
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
            TaskUpdatedText.SetActive(false);
        }
        else if (controls.UI.TaskMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }
        else if (controls.UI.PauseMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }

        MainTask = "";
        if (TaskFinished[0] == false)
        {
            MainTask = "<b>Tear out power cells. (" + PowerCellsRemoved.ToString() + "/6)</b>";
        }
        else
        {
            MainTask = "<b>Push the poorly labled button.</b>";
        }



        taskList.text = MainTask + "\n";


        if (PowerCellsRemoved >= 6)
            TaskCompleted("Power");



    }


    public void TaskCompleted(string Task)
    {
        switch (Task)
        {
            case "Power":
                if (TaskFinished[0] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                    StartCoroutine(confettistuff());
                    Destroy(Door);

                }
                break;
            case "Button":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());
                    Tasks[1].SetActive(true);
                    NextLevel.SetActive(true);
                }
                break;
           
        }

    }

    IEnumerator confettistuff()
    {
        confetti.SetActive(true);
        yield return new WaitForSeconds(2);
        confetti.SetActive(false);
    }

    public void LightButton()
    {
        if (PowerCellsRemoved == 1)
            b1.GetComponent<MeshRenderer>().material = CompletedMat;
        if (PowerCellsRemoved == 2)
            b2.GetComponent<MeshRenderer>().material = CompletedMat;
        if (PowerCellsRemoved == 3)
            b3.GetComponent<MeshRenderer>().material = CompletedMat;
        if (PowerCellsRemoved == 4)
            b4.GetComponent<MeshRenderer>().material = CompletedMat;
        if (PowerCellsRemoved == 5)
            b5.GetComponent<MeshRenderer>().material = CompletedMat;
        if (PowerCellsRemoved == 6)
            b6.GetComponent<MeshRenderer>().material = CompletedMat;
    }
}
