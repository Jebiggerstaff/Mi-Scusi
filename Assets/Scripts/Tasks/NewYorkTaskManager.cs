using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewYorkTaskManager : MonoBehaviour
{

    //Public Variables
    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    [Header("Collision Boxes")]
    public GameObject FrontOfLine;
    public GameObject[] SSSTags = new GameObject[0];
    public GameObject[] ScusiTags = new GameObject[0];
    public GameObject Builder;

    [Header("Task Objects")]

    public GameObject[] MansLuggage = new GameObject[0];
    public GameObject[] PoliticalSigns = new GameObject[0];
    public GameObject[] ParkTrash = new GameObject[0];
    public GameObject[] MartBikes = new GameObject[0];

    public GameObject NextLevelBubble;

    [HideInInspector] public int SSStagsRemoved = 0;
    [HideInInspector] public int TrashPickedUp = 0;
    [HideInInspector] public int WindowsBroken = 0;
    [HideInInspector] public int BikesReturned = 0;
    [HideInInspector] public int ObjectsBroughtToVan = 0;
    [HideInInspector] public int SignsMoved = 0;
    [HideInInspector] public bool AteAtCafe=false;

    [Header("Audio Clips")]
    public AudioClip genericCompletionClip;
    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

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
        RandomAudioMaker.makeAudio(genericCompletionClip);
        TaskCompleteText.SetActive(true);
        switch (Task)
        {
            case "CutInLine":
                Tasks[0].SetActive(true);
                break;
            case "HelpGuyMove":
                Tasks[1].SetActive(true);
                NextLevelBubble.SetActive(true);
                break;
            case "RemovePolitical":
                Tasks[2].SetActive(true);
                break;
            case "PickUpTrash":
                Tasks[3].SetActive(true);
                break;
            case "DefaceSSS":
                Tasks[4].SetActive(true);
                break;
            case "ShatterWindows":
                Tasks[5].SetActive(true);
                break;
            case "EatAtCafe":
                Tasks[6].SetActive(true);
                break;
            case "CrossStreet":
                Tasks[7].SetActive(true);
                break;
            case "ReturnBlueBikes":
                Tasks[8].SetActive(true);
                break;
            case "DefaceHQ":
                Tasks[9].SetActive(true);
                break;
            case "BringFood":
                Tasks[10].SetActive(true);
                break;
        }

    }

}