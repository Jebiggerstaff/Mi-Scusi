using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewYorkTaskCollider : MonoBehaviour
{
    public NewYorkTaskManager NewYorkTaskManager;

    static public bool CrossingStreet;
    static public bool CrossedStreet = false;

    public void Start()
    {
        NewYorkTaskManager = GameObject.Find("TaskUI").GetComponent<NewYorkTaskManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        #region CrossingRoad
        if (collision.gameObject.name == "APR_Body" && name == "Car")
        {
            //Debug.Log("Hit By Car");
            CrossingStreet = false;
        }
        #endregion
    }

    public void OnTriggerEnter(Collider other)
    {
        #region SSStags
        if (other.name == "APR_Head" && name == "SSSTag")
        {

            int index = System.Array.IndexOf(NewYorkTaskManager.SSSTags,this.gameObject);
            NewYorkTaskManager.ScusiTags[index].SetActive(true);
            NewYorkTaskManager.SSStagsRemoved++;
            if (NewYorkTaskManager.SSStagsRemoved == NewYorkTaskManager.SSSTags.Length)
            {
                NewYorkTaskManager.TaskCompleted("DefaceSSS");
            }
            Destroy(this.gameObject);
        }
        #endregion
        #region LineCut
        if (other.name == "APR_Head" && name == "LineFront")
        {
            NewYorkTaskManager.TaskCompleted("CutInLine");
        }
        #endregion
        #region CrossingRoad
        if (other.name == "APR_Head" && name == "RoadS")
        {
            CrossingStreet = true;
            Debug.Log("Crossing Street");
        }
        else if (other.name == "APR_Head" && name == "RoadN")
        {
            if (CrossingStreet == true && !CrossedStreet)
            {
                CrossedStreet = true;
                NewYorkTaskManager.TaskCompleted("CrossStreet");
            }
        }
        #endregion
        #region PickUpTrash
        if (other.name == "TrashBag" && name == "InsideTrashBin")
        {
            NewYorkTaskManager.TrashPickedUp++;

            if (NewYorkTaskManager.TrashPickedUp == NewYorkTaskManager.ParkTrash.Length)
            {
                NewYorkTaskManager.TaskCompleted("PickUpTrash");
            }
        }
        #endregion
        #region ReturnBikes
        if (other.name == "Bike" && name == "BikeZone")
        {
            NewYorkTaskManager.BikesReturned++;

            if (NewYorkTaskManager.BikesReturned == NewYorkTaskManager.MartBikes.Length)
            {
                NewYorkTaskManager.TaskCompleted("ReturnBlueBikes");
            }
        }
        #endregion
        #region HelpGuyMove
        if (NewYorkTaskManager.MansLuggage.Contains(other.gameObject) && name == "VanInterior")
        {
            NewYorkTaskManager.ObjectsBroughtToVan++;

            if (NewYorkTaskManager.ObjectsBroughtToVan == 3)
            {
                NewYorkTaskManager.TaskCompleted("HelpGuyMove");
            }
        }
        #endregion
        #region DefaceHQ
        if (other.name == "APR_Head" && name == "DefaceHQZone")
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            NewYorkTaskManager.TaskCompleted("DefaceSSS");
        }
        #endregion
        #region DeliverFood
        if (other.name == "CafeFood" && name == "Food Delivery Collider")
        {
            NewYorkTaskManager.TaskCompleted("BringFood");
        }
        #endregion
    }
    public void OnTriggerExit(Collider other)
    {
        #region PickUpTrash
        if (other.name == "TrashBag" && name == "InsideTrashBin")
        {
            NewYorkTaskManager.TrashPickedUp--;

        }
        #endregion
        #region ReturnBikes
        if (other.name == "Bike" && name == "BikeZone")
        {
            NewYorkTaskManager.BikesReturned--;

        }
        #endregion
        #region HelpGuyMove
        if (NewYorkTaskManager.MansLuggage.Contains(other.gameObject) && name == "VanInterior")
        {
            NewYorkTaskManager.ObjectsBroughtToVan--;

        }
        #endregion
        #region RemovePolitical
        if (NewYorkTaskManager.PoliticalSigns.Contains(other.gameObject) && name == "SignZone")
        {
            NewYorkTaskManager.SignsMoved++;

            if(NewYorkTaskManager.SignsMoved == 5)
            {
                NewYorkTaskManager.TaskCompleted("RemovePolitical");
            }

        }
        #endregion

    }
}
