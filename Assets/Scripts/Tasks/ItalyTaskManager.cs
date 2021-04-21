using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ItalyTaskManager : MonoBehaviour
{

    public GameObject confetti;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskUpdatedText;
    public GameObject TaskList;
    public GameObject NextLevel;
    public GameObject GiantMeatball;
    public GameObject MeatballEffect;
    public NPC ChefMiti;
    public ItalyTaskCollider fountainCollider;
    public GameObject mafiaman;

    public Text taskList;

    public CosmeticUnlocker CosmeticUnlocker;

    [HideInInspector] public bool PunchedMafia = false;
    [HideInInspector] public bool PunchedCustomer = false;
    [HideInInspector] public bool AteSpaghetti = false;
    [HideInInspector] public int DocumentsCollected = 0;

    private bool[] TaskFinished = new bool[11];
    private bool menuOpen;
    private string CurrentMainTask = "<b>Beat up men in suits</b>\n";

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;
    public AudioClip MeatballSound;
    public AudioClip DocumentGrabbedNoise;

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
        else if (controls.UI.PauseMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }

        if (TaskFinished[2] == true)
        {
            CurrentMainTask = "<b>Collect 5 Mafia documents (" + DocumentsCollected.ToString() + "/5)</b>\n";
        }

        taskList.text = CurrentMainTask +
            "Give upset girl a flower\n" +
            "Eat spaghetti\n" +
            "Steal from the shops\n" +
            "Get money from the fountain (" + (fountainCollider.CoinsInFountain / 2 + 3).ToString() + "/3)\n" +
            "Knock out angry customer\n" +
            "Help Chef Miti make meatball (" + MiniMeatball.numMeatballsCollected.ToString() + "/3)";
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
                    CurrentMainTask = CurrentMainTask = "<b>Infiltrate Mafia HQ</b>\n";
                    StartCoroutine(confettistuff());
                }
                break;
            case "BeatUpMafiaMembers":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                    CurrentMainTask = CurrentMainTask = "<b>Speak to the lone Mafia Member</b>\n";

                    CosmeticUnlocker.UnlockOutfit("Fedora");
                    CosmeticUnlocker.UnlockOutfit("Mafia Coat");
                    CosmeticUnlocker.UnlockOutfit("Mafia Shirt");
                    CosmeticUnlocker.UnlockOutfit("Mafia Pants");
                    StartCoroutine(confettistuff());

                    mafiaman.SetActive(true);
                }
                break;
            case "InfiltrateMafiaHQ":
                if (TaskFinished[2] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[2].SetActive(true);
                    TaskFinished[2] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "Collect5documents":
                if (TaskFinished[3] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[0].SetActive(true);
                    TaskFinished[3] = true;
                    NextLevel.SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "FlowersToGirl":
                if (TaskFinished[5] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Flower");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[5].SetActive(true);
                    TaskFinished[5] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "EatSpaghetti":
                if (TaskFinished[6] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[6].SetActive(true);
                    TaskFinished[6] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "StealFromShop":
                if (TaskFinished[7] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "GetMoneyFromFountain":
                if (TaskFinished[8] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Treasure");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[8].SetActive(true);
                    TaskFinished[8] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "KnockoutAngryCustomer":
                if (TaskFinished[9] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[9].SetActive(true);
                    TaskFinished[9] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "GiantMeatball":
                if(TaskFinished[10] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Chef's Hat");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[10].SetActive(true);
                    TaskFinished[10] = true;

                    GiantMeatball.SetActive(true);
                    MeatballEffect.SetActive(true);
                    RandomAudioMaker.makeAudio(MeatballSound);

                    ChefMiti.sentences = new string[1];
                    ChefMiti.sentences[0] = "Fantastico! That is a the greatest meatball I have ever a made!";
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
