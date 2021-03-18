using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTaskManager : MonoBehaviour
{

    public GameObject confetti;

    //Public Variables
    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];
    bool[] TasksCompleted;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;
    public NPC DustyTheJanitor;
    public NPC JaniceTheReceptionist;

    [Header("Task Objects")]

    public GameObject WaterCoolerJug;
    public GameObject Coffee;

    public GameObject PaperStack1;
    public GameObject PaperStack2;
    public GameObject PaperStack3;

    public GameObject PhoneNumberPrefab;

    public GameObject coffeeObject1;
    public GameObject coffeeObject2;
    public GameObject coffeeObject3;
    public GameObject CoffeePrefab;

    public GameObject NextLevel;

    public int coffeePartsCollected=0;
    bool CoffeeSpawned = false;

    public CosmeticUnlocker CosmeticUnlocker;
    private bool menuOpen;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

    }

    void FixedUpdate()
    {
        if (!CoffeeSpawned && coffeePartsCollected >= 3)
        {
            Destroy(coffeeObject1);
            Destroy(coffeeObject2);
            Destroy(coffeeObject3);
            CoffeeSpawned = true;
            Instantiate(CoffeePrefab, new Vector3(54.8100014f, 34.8970032f, 10.0401859f), new Quaternion(0, 0, 0, 0));
        }
    }
    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
        TasksCompleted = new bool[Tasks.Length];
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
        TaskCompleteText.SetActive(true);
        switch (Task)
        {
            case "RefilWater":
                if (TasksCompleted[0] == false)
                {
                    TasksCompleted[0] = true;
                    Tasks[0].SetActive(true);
                    PaperStack1.SetActive(true);
                    DustyTheJanitor.sentences = new string[2];
                    DustyTheJanitor.sentences[0] = "Whelp, much obliged new guy!";
                    DustyTheJanitor.sentences[1] = "Well, I think I'm gonna take a nap now, so see yah later.";

                    JaniceTheReceptionist.sentences = new string[2];
                    JaniceTheReceptionist.sentences[0] = "Did you help Barnaby?";
                    JaniceTheReceptionist.sentences[1] = "Very good, head upstairs, I'm sure someone could use your help.";
                    StartCoroutine(confettistuff());
                }
                break;
            case "HelpCoworker":
                if (TasksCompleted[1] == false)
                {
                    TasksCompleted[1] = true;
                    Tasks[1].SetActive(true);
                    PaperStack2.SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "GetBossCoffee":
                if (TasksCompleted[2] == false)
                {
                    TasksCompleted[2] = true;
                    CosmeticUnlocker.UnlockOutfit("Mustache");
                    Tasks[2].SetActive(true);
                    PaperStack3.SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "CopyButt":
                if (TasksCompleted[3] == false)
                {
                    TasksCompleted[3] = true;
                    Tasks[3].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "BossStache":
                if (TasksCompleted[4] == false)
                {
                    TasksCompleted[4] = true;
                    Tasks[4].SetActive(true);
                    NextLevel.SetActive(true);
                    CosmeticUnlocker.UnlockOutfit("Mustache");
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
