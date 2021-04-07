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
    public GameObject PrincessObjects;
    public GameObject SSSObjects;
    public GameObject MacObjects;
    public GameObject AlienObjects;
    public GameObject BurningObjects;
    [Space]
    public CosmeticUnlocker CosmeticUnlocker;

    [HideInInspector] public bool[] TaskFinished;
    private bool menuOpen;


    string MainTask;
    string SlayPrincess = "Complete other side tasks in the game to unlock this side task!";
    string SlaySSS = "Complete other side tasks in the game to unlock this side task!";
    string SlayMafia = "Complete other side tasks in the game to unlock this side task!";
    string SaveAliens = "Complete other side tasks in the game to unlock this side task!";
    string SaveMacaroni = "Complete other side tasks in the game to unlock this side task!";
    string BurnBurningMan = "Complete other side tasks in the game to unlock this side task!";


    public static string CompletedExtras = "CompletedExtras";

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

        menuOpen = true;

        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Aliens"))
        {
            SaveAliens = "Save your alien pals from captivity!";

            if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Princess"))
            {
                SlayPrincess = "Save the princess...from existing!";
            }
            else
            {
                PrincessObjects.SetActive(false);
            }
            if (PlayerPrefs.GetString(CompletedExtras, "").Contains("SSS"))
            {
                SlaySSS = "Defeat your street tagging nemesis...once and for all!";
            }
            else
            {
                SSSObjects.SetActive(false);
            }
            SlayMafia = "Finish off the mafia boss!";
            if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Macaroni"))
            {
                SaveMacaroni = "Help your macaroni friend see the WHOLE world!";
            }
            else
            {
                MacObjects.SetActive(false);
            }
        }
        else
        {
            AlienObjects.SetActive(false);
            MacObjects.SetActive(false);
            SSSObjects.SetActive(false);
            PrincessObjects.SetActive(false);
        }
        if (PlayerPrefs.GetString(CompletedExtras, "").Contains("Burning"))
        {
            BurnBurningMan = "Burn down burning man...again!";
        }
        else
        {
            BurningObjects.SetActive(false);
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
            MainTask = "Launch the rocket!";
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
                if (TaskFinished[0] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "Rocket":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());
                    

                    NextLevel.SetActive(true);
                }
                break;
            case "Aliens":
                if (TaskFinished[2] == false)
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
                if (TaskFinished[3] == false)
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
                if (TaskFinished[4] == false)
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
                if (TaskFinished[5] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[5].SetActive(true);
                    TaskFinished[5] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "SSS":
                if (TaskFinished[6] == false)
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
                if (TaskFinished[7] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
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
