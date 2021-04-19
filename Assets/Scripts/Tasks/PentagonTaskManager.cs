using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PentagonTaskManager : MonoBehaviour
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
    public GameObject[] PrincessObjects;
    public GameObject[] SSSObjects;
    public GameObject MacObjects;
    public GameObject AlienObjects;
    public GameObject BurningObjects;
    [Space]
    public GameObject DoorOne;
    public GameObject DoorTwo;
    public GameObject[] BurningManFire;
    public GameObject LaunchParticles;
    [Space]
    public CosmeticUnlocker CosmeticUnlocker;

    [HideInInspector] public bool[] TaskFinished;
    [HideInInspector] public bool[] TaskAllowed;
    private bool menuOpen;


    string MainTask;
    string SlayPrincess = "Complete other tasks in the game to unlock this!";
    string SlaySSS = "Complete other tasks in the game to unlock this!";
    string SlayMafia = "Complete other tasks in the game to unlock this!";
    string SaveAliens = "Complete other tasks in the game to unlock this!";
    string SaveMacaroni = "Complete other tasks in the game to unlock this!";
    string BurnBurningMan = "Complete other tasks in the game to unlock this!";


    public static string CompletedExtras = "CompletedExtras";

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;
    public AudioClip ShuttleLaunch;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

        TaskFinished = new bool[Tasks.Length];
        TaskAllowed = new bool[Tasks.Length];
        for (int i = 0; i < TaskFinished.Length; i++)
        {
            TaskFinished[i] = false;
            TaskAllowed[i] = true;
        }

        menuOpen = true;

        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Aliens"))
        {
            SaveAliens = "Save your alien pals from captivity!";
        }
        else
        {
            AlienObjects.SetActive(false);
            TaskAllowed[2] = false;
        }

        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Princess"))
        {
            SlayPrincess = "Save the princess...from existing!";
        }
        else
        {
            foreach(var go in PrincessObjects)
                go.SetActive(false);
            TaskAllowed[3] = false;
        }

        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("SSS"))
        {
            SlaySSS = "Defeat your street tagging nemesis...once and for all!";
        }
        else
        {
            foreach (var go in SSSObjects)
                go.SetActive(false);
            TaskAllowed[6] = false;
        }

        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Macaroni"))
        {
            SaveMacaroni = "Help your macaroni friend see the WHOLE world!";
        }
        else
        {
            MacObjects.SetActive(false);
            TaskAllowed[5] = false;
        }

        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Burning"))
        {
            BurnBurningMan = "Burn down burning man...again!";
        }
        else
        {
            BurningObjects.SetActive(false);
            TaskAllowed[7] = false;
        }

        SlayMafia = "Finish off the mafia boss!";

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

        MainTask = "";
        if (TaskFinished[0] == false)
        {
            MainTask = "Open the door to the launch bay.";
        }
        else
        {
            MainTask = "Launch the rocket with a mighty punch!";
        }

        taskList.text = MainTask + "\n" +
            SaveAliens + "\n" + 
            SlayPrincess + "\n" + 
            SlayMafia + "\n" + 
            SaveMacaroni + "\n" +
            SlaySSS + "\n" + 
            BurnBurningMan;

        


    }
    
    public void TaskCompleted(string Task)
    {
        switch (Task)
        {
            case "Door":
                if (TaskFinished[0] == false && TaskAllowed[0])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                    StartCoroutine(confettistuff());


                    Destroy(DoorOne);
                    Destroy(DoorTwo);
                }
                break;
            case "Rocket":
                if (TaskFinished[1] == false && TaskAllowed[1])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());

                    LaunchParticles.SetActive(true);

                    NextLevel.SetActive(true);

                    RandomAudioMaker.makeAudio(ShuttleLaunch, 0.5f);
                }
                break;
            case "Aliens":
                if (TaskFinished[2] == false && TaskAllowed[2])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[2].SetActive(true);
                    TaskFinished[2] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "Princess":
                if (TaskFinished[3] == false && TaskAllowed[3])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[3].SetActive(true);
                    TaskFinished[3] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "Mafia":
                if (TaskFinished[4] == false && TaskAllowed[4])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[4].SetActive(true);
                    TaskFinished[4] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "Macaroni":
                if (TaskFinished[5] == false && TaskAllowed[5])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[5].SetActive(true);
                    TaskFinished[5] = true;
                    StartCoroutine(confettistuff());

                    if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Penroni") == false)
                    {
                        PlayerPrefs.SetString(CompletedExtras, PlayerPrefs.GetString(CompletedExtras, "") + "Penroni");
                        PlayerPrefs.Save();
                    }
                }
                break;
            case "SSS":
                if (TaskFinished[6] == false && TaskAllowed[6])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[6].SetActive(true);
                    TaskFinished[6] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "BurningMan":
                if (TaskFinished[7] == false && TaskAllowed[7])
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
                    StartCoroutine(confettistuff());

                    foreach (var go in BurningManFire)
                    {
                        go.SetActive(true);
                    }

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
