﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTaskManager : MonoBehaviour
{
    //Public Variables
    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskList;

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

    public int coffeePartsCollected=0;
    bool CoffeeSpawned = false;


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
        TaskCompleteText.SetActive(true);
        switch (Task)
        {
            case "RefilWater":
                Tasks[0].SetActive(true);
                PaperStack1.SetActive(true);
                break;
            case "HelpCoworker":
                Tasks[1].SetActive(true);
                PaperStack2.SetActive(true);
                break;
            case "GetBossCoffee":
                Tasks[2].SetActive(true);
                PaperStack3.SetActive(true);
                break;
            case "CopyButt":
                Tasks[3].SetActive(true);
                break;
        }

    }
}
