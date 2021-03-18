using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskManager : MonoBehaviour
{

    public GameObject confetti;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];
    bool[] TasksCompleted;
    private bool menuOpen;

    [HideInInspector] public bool Pickedup = false;
    [HideInInspector] public bool Punched=false;

    void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
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
            case "GrabSomething":
                if (TasksCompleted[0] == false)
                {
                    TasksCompleted[0] = true;
                    Tasks[0].SetActive(true);
                    StartCoroutine(confettistuff());
                }
                break;
            case "PunchAGuy":
                if (TasksCompleted[1] == false)
                {
                    TasksCompleted[1] = true;
                    Tasks[1].SetActive(true);
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
