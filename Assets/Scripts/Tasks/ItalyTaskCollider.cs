using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItalyTaskCollider : MonoBehaviour
{
    ItalyTaskManager ItalyTaskManager = new ItalyTaskManager();

    private int CoinsMoved;

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

    }
    private void OnTriggerExit(Collider other)
    {
        #region GetMoneyFromFountain
        if (other.name == "Coin" && name == "FountainInterior")
        {
            CoinsMoved++;
            if (CoinsMoved == 3)
            {
                ItalyTaskManager.TaskCompleted("GetMoneyFromFountain");
            }
        }
        #endregion
    }
}
