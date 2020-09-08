using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewYorkTaskManager : MonoBehaviour
{

    //Public Variables
    [Header("Tasks")]
    public bool CutInLine = false;
    public bool HelpedManMove =false;
    public bool RemovePoliticalSigns = false;
    public bool PickUpTrashInPark = false;
    public bool DefaceSSSTags = false;
    public bool Shatter5Windows = false;
    public bool EatAtCafe = false;
    public bool CrossTheStreet = false;
    public bool ReturnBlueBikes = false;
    public bool DefaceCorperateHQ = false;
    public bool BringFoodToBuilders = false;

    [Header("Player")]
    public GameObject Player;

    [Header("Collision Boxes")]
    public GameObject FrontOfLine;
    public GameObject[] SSSTags = new GameObject[0];
    public GameObject[] ShatterableWindows = new GameObject[0];
    public GameObject Cafe;
    public GameObject OtherSideOfStreet;
    public GameObject BikeRack;
    public GameObject[] CorperateHQs = new GameObject[0];
    public GameObject Builders;

    [Header("Task Objects")]
    public GameObject[] MansLuggage = new GameObject[0];
    public GameObject[] PoliticalSigns = new GameObject[0];
    public GameObject[] ParkTrash = new GameObject[0];
    public GameObject[] MartBikes = new GameObject[0];
    public GameObject[] CafeFood = new GameObject[0];

    [HideInInspector]
    public int SSStagsRemoved=0;

    //Private Variables
    private GameObject TaskCompleteText;

    public void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        Player = GameObject.Find("FinalPlayer");

        TaskCompleteText = transform.GetChild(0).gameObject;
    }

    public void TaskCompleted()
    {
        TaskCompleteText.gameObject.SetActive(true);
        StartCoroutine("TaskCompletedEnd", 5f);
    }  
    private void TaskCompletedEnd()
    {
        TaskCompleteText.gameObject.SetActive(false);
    }
}
