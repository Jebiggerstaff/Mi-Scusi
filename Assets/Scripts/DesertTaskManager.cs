using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class DesertTaskManager : MonoBehaviour
{

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskUpdatedText;
    public GameObject TaskList;
    public GameObject NextLevel;

    public Text taskList;

    public CosmeticUnlocker CosmeticUnlocker;

    [HideInInspector] public bool PunchedMafia = false;
    [HideInInspector] public bool PunchedCustomer = false;
    [HideInInspector] public bool AteSpaghetti = false;
    [HideInInspector] public int DocumentsCollected = 0;

    private bool[] TaskFinished;
    private bool menuOpen;


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

        
    }


    public void TaskCompleted(string Task)
    {

        switch (Task)
        {
            case "HearAboutMafia":
                if (TaskFinished[0] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                }
                break;

        }
    }
}

