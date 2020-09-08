using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    //Private Variables

    void FixedUpdate()
    {
        
    }
}
