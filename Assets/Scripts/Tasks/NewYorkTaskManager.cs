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
    bool[] CompletedTasks;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    public CosmeticUnlocker CosmeticUnlocker;

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
        CompletedTasks = new bool[Tasks.Length];
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
        if (CosmeticUnlocker == null)
            CosmeticUnlocker = FindObjectOfType<OverlayScene>().menu.GetComponent<CosmeticUnlocker>();

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
                if (CompletedTasks[0] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Pants");
                    Tasks[0].SetActive(true);
                    CompletedTasks[0] = true;
                }
                break;
            case "HelpGuyMove":
                if (CompletedTasks[1] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Generic");
                    Tasks[1].SetActive(true);
                    NextLevelBubble.SetActive(true);

                    CompletedTasks[1] = true;
                }
                    break;
            case "RemovePolitical":
                if (CompletedTasks[2] == false)
                {

                    CompletedTasks[2] = true;
                    Tasks[2].SetActive(true);
                }
                break;
            case "PickUpTrash":
                if (CompletedTasks[3] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Jeans");
                    Tasks[3].SetActive(true);
                    CompletedTasks[3] = true;
                }
                break;
            case "DefaceSSS":
                if (CompletedTasks[4] == false)
                {
                    Tasks[4].SetActive(true);
                    CompletedTasks[4] = true;
                }
                break;
            case "ShatterWindows":
                if (CompletedTasks[5] == false)
                {
                    Tasks[5].SetActive(true);
                    CompletedTasks[5] = true;
                }
                break;
            case "EatAtCafe":
                if (CompletedTasks[6] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Shorts");
                    Tasks[6].SetActive(true);
                    CompletedTasks[6] = true;
                }
                break;
            case "CrossStreet":
                if (CompletedTasks[7] == false)
                {
                    Tasks[7].SetActive(true);
                    CompletedTasks[7] = true;
                }
                break;
            case "ReturnBlueBikes":
                if (CompletedTasks[8] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("GenericBP");
                    Tasks[8].SetActive(true);
                    CompletedTasks[8] = true;
                }
                break;
            case "DefaceHQ":
                if (CompletedTasks[9] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Expedition");
                    CompletedTasks[9] = true;
                    Tasks[9].SetActive(true);
                }
                break;
            case "BringFood":
                if (CompletedTasks[10] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("PuffyCoat");
                    CompletedTasks[10] = true;
                    Tasks[10].SetActive(true);
                }
                break;
        }

    }

}