using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoirTaskManager : MonoBehaviour
{

    public GameObject confetti;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;
    public GameObject NextLevel;

    public CosmeticUnlocker CosmeticUnlocker;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];
    public bool[] TasksCompleted;

    [Header("Murder GameObjects")]
    public GameObject[] Suspects = new GameObject[0];
    public GameObject MurderWeapon;
    private bool menuOpen=true;

    [HideInInspector] public bool LevelCompleted;

    void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        TasksCompleted = new bool[Tasks.Length];
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

        if (TasksCompleted[1] && TasksCompleted[0] && TasksCompleted[1])
        {
            NextLevel.SetActive(true);
            LevelCompleted = true;
        }
    }

    public void TaskCompleted(string Task)
    {
        TaskCompleteText.SetActive(true);
        switch (Task)
        {
            case "MurderWeapon":
                if(TasksCompleted[1] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Sword");
                    TasksCompleted[1] = true;
                    Tasks[1].SetActive(true);
                    StartCoroutine(confettistuff());
                }   
                break;
            case "MurderSuspect":
                if (TasksCompleted[0] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Pipe");
                    TasksCompleted[0] = true;
                    Tasks[0].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "WantedPoster":
                if (TasksCompleted[2] == false)
                {
                    TasksCompleted[2] = true;
                    Tasks[2].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "Sevens":
                if (TasksCompleted[3] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Fur Coat");
                    TasksCompleted[3] = true;
                    Tasks[3].SetActive(true);
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
