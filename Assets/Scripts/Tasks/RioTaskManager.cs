using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RioTaskManager : MonoBehaviour
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
    public GameObject Scooter;
    public GameObject Sword;
    public GameObject Coin;
    public GameObject UFO;
    public GameObject Bike;
    public GameObject miniTrophy;

    [HideInInspector] public int peopleHelped;
    [Space]
    public List<GameObject> GoalConfetti;
    public GameObject hammer;
    [Space]


    public CosmeticUnlocker CosmeticUnlocker;
    
    [HideInInspector] public bool[] TaskFinished;
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
            MainTask = "<b>Break into the locker room.</b>";
        }
        else if(TaskFinished[1] == false)
        {
            MainTask = "<b>Put on a player's outfit.</b>";
        }
        else if(TaskFinished[2] == false)
        {
            MainTask = "<b>Score a goal in the big game</b>";
        }
        else
        {
            MainTask = "<b>Escape with the trophy!</b>";
        }



        taskList.text = MainTask + "\n" +
            "Find the hidden booty of Rio\n" +
            "Help your friendly neighborhood crime lord\n" + 
            "Get the ball rolling\n" +
            "Help everyone go shopping (" + peopleHelped.ToString() + "/7)";


        if (!TaskFinished[1])
        {
            if (PlayerPrefs.GetInt("Costume_Shirt", 0) == 6 && PlayerPrefs.GetInt("Costume_Pants", 0) == 7)
                TaskCompleted("Outfit");
        }

        

    }


    public void TaskCompleted(string Task)
    {
        switch (Task)
        {
            case "LockerRoom":
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
            case "Outfit":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());
                    
                    
                }
                break;
            case "Goal":
                if (TaskFinished[2] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    TaskFinished[2] = true;
                    StartCoroutine(confettistuff());

                    foreach(var go in GoalConfetti)
                    {
                        go.SetActive(true);
                        
                    }
                    hammer.SetActive(true);

                    foreach(var ai in FindObjectsOfType<RioCrowdAI>())
                    {
                        ai.GoalMade();
                    }
                    
                }
                break;
            case "Getaway":
                if (TaskFinished[3] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[3].SetActive(true);
                    TaskFinished[3] = true;
                    StartCoroutine(confettistuff());


                    NextLevel.SetActive(true);
                }
                break;
            case "Booty":
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
            case "CrimeLord":
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
            case "Ball":
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
            case "Shopping":
                if (TaskFinished[7] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
                    StartCoroutine(confettistuff());

                    CosmeticUnlocker.UnlockOutfit("Satchel");

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
