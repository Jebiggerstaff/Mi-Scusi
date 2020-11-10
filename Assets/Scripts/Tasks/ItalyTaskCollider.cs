using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItalyTaskCollider : MonoBehaviour
{
    ItalyTaskManager ItalyTaskManager = new ItalyTaskManager();

    private int CoinsInFountain;


    public void Start()
    {
        ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();
    }

    public void OnTriggerEnter(Collider other)
    {

        #region InfiltrateMafiaHQ
        if (other.name == "APR_Head" && name == "MafiaHQHitBox")
        {
            ItalyTaskManager.TaskCompleted("InfiltrateMafiaHQ");
        }
        #endregion
        #region knockFisherman
        if (other.name == "FishermanRoot" && name == "Water")
        {
            ItalyTaskManager.TaskCompleted("KnockFishermanIntoWater");
        }
        #endregion
        #region FlowersToGirl
        if (other.name == "Flower" && name == "girlRoot")
        {
            ItalyTaskManager.TaskCompleted("FlowersToGirl");
        }
        #endregion
        #region GetMoneyFromFountain
        if (other.name == "Coin" && name == "FountainInterior")
        {
            CoinsInFountain--;
        }
        #endregion


    }
    private void OnTriggerExit(Collider other)
    {
        #region GetMoneyFromFountain
        if (other.name == "Coin" && name == "FountainInterior")
        {
            CoinsInFountain++;
            if (CoinsInFountain == 0)
            {
                ItalyTaskManager.TaskCompleted("GetMoneyFromFountain");
            }
        }
        #endregion
        #region StealFromStores
        if ((other.name == "Cheese"||other.name=="Scooter"||other.name=="Apple") && name == "StoreInterior")
        {
            ItalyTaskManager.TaskCompleted("StealFromShop");
        }
        #endregion
    }
}
