using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewYorkTaskManager : MonoBehaviour
{

    public GameObject confetti;

    //Public Variables
    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];
    bool[] CompletedTasks;
    private bool menuOpen;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    public GameObject hostileSam;
    public GameObject normalSam;

    public Text taskText;

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

        taskText.text = "<b>Get lucky 777's</b>\n"+
            "<b>Help man at TechHQ move</b>\n" +
            "Remove the political competition\n" +
            "Pick up the trash bags in the park (" + TrashPickedUp.ToString() + "/" + ParkTrash.Length.ToString() + ")\n" +
            "Deface Street Slick Sam's 6 Tags (" + SSStagsRemoved.ToString() + "/6)\n" +
            "Shatter 15 windows (" + WindowsBroken.ToString() + "/15)\n" +
            "Eat at the South West cafe\n" +
            "Cross the first street\n" +
            "Return the 3 missing Blue Bikes (" + BikesReturned.ToString() + "/3)\n" +
            "Deface the Tri-Gon headquarters\n" +
            "Bring food to the construction worker\n" +
            "Cut in line for a hot dog"
            ;
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
                    CosmeticUnlocker.UnlockOutfit("Tan Pants");
                    Tasks[0].SetActive(true);
                    CompletedTasks[0] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "HelpGuyMove":
                if (CompletedTasks[1] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Generic Shirt");
                    Tasks[1].SetActive(true);
                    NextLevelBubble.SetActive(true);

                    CompletedTasks[1] = true;
                    StartCoroutine(confettistuff());
                }
                    break;
            case "RemovePolitical":
                if (CompletedTasks[2] == false)
                {
                    CompletedTasks[2] = true;
                    Tasks[2].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "PickUpTrash":
                if (CompletedTasks[3] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Jeans");
                    Tasks[3].SetActive(true);
                    CompletedTasks[3] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "DefaceSSS":
                if (CompletedTasks[4] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Headphones");
                    Tasks[4].SetActive(true);
                    CompletedTasks[4] = true;
                    StartCoroutine(confettistuff());
                    hostileSam.SetActive(true);
                    normalSam.SetActive(false);

                    if(PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "").Contains("SSS") == false)
                    {
                        PlayerPrefs.SetString(PentagonTaskManager.CompletedExtras, PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "") + "SSS");
                    }
                }
                break;
            case "ShatterWindows":
                if (CompletedTasks[5] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Poofy Hair");
                    Tasks[5].SetActive(true);
                    CompletedTasks[5] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "EatAtCafe":
                if (CompletedTasks[6] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Tan Shorts");
                    Tasks[6].SetActive(true);
                    CompletedTasks[6] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "CrossStreet":
                if (CompletedTasks[7] == false)
                {
                    Tasks[7].SetActive(true);
                    CompletedTasks[7] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "ReturnBlueBikes":
                if (CompletedTasks[8] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Generic");
                    Tasks[8].SetActive(true);
                    CompletedTasks[8] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "DefaceHQ":
                if (CompletedTasks[9] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Expedition");
                    CompletedTasks[9] = true;
                    Tasks[9].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "BringFood":
                if (CompletedTasks[10] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Puffy Coat");
                    CompletedTasks[10] = true;
                    Tasks[10].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "777":
                if(CompletedTasks[11] == false)
                {
                    CompletedTasks[11] = true;
                    Tasks[11].SetActive(true);
                    StartCoroutine(confettistuff());
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

}