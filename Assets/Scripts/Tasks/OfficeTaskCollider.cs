using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTaskCollider : MonoBehaviour
{

    public OfficeTaskManager OfficeTaskManager;
    bool PaperSpawned = false;
    public GameObject FullWaterJug;


    // Start is called before the first frame update
    void Start()
    {
        OfficeTaskManager = GameObject.Find("TaskUI").GetComponent<OfficeTaskManager>();
    }


    public void OnTriggerEnter(Collider other)
    {
        #region WaterCooler
        if (other.gameObject == OfficeTaskManager.WaterCoolerJug && name == "WaterCooler")
        {
            OfficeTaskManager.TaskCompleted("RefilWater");
        }
        if (other.gameObject.name == "WaterJug" )
        {
            FullWaterJug.SetActive(true);
            Destroy(other.gameObject);
        }
        #endregion
        #region OfficeDouche
        if (other.name == "APR_Head" && name == "OfficeDouche")
        {
            if (!PaperSpawned)
            {
                Instantiate(OfficeTaskManager.PhoneNumberPrefab, new Vector3(65.61f, 34.63f, -23.56f), new Quaternion(0, 0, 0, 0));
                PaperSpawned = true;
            }
        }
        #endregion
        #region PhoneNumberDelivery
        if (other.name == "PhoneNumber(Clone)" && name == "Secretary")
        {
            OfficeTaskManager.TaskCompleted("HelpCoworker");
        }
        #endregion
        #region MakeCoffee
        if (other.gameObject == OfficeTaskManager.coffeeObject1 && name == "CoffeMaker")
        {
            OfficeTaskManager.coffeePartsCollected++;
        }
        if (other.gameObject == OfficeTaskManager.coffeeObject2 && name == "CoffeMaker")
        {
            OfficeTaskManager.coffeePartsCollected++;
        }
        if (other.gameObject == OfficeTaskManager.coffeeObject3 && name == "CoffeMaker")
        {
            OfficeTaskManager.coffeePartsCollected++;
        }
        #endregion
        #region CoffeeToBoss
        if (other.name == "CoffeeMugBoss" && name == "Boss")
        {
            OfficeTaskManager.TaskCompleted("GetBossCoffee");
        }
        #endregion
        #region CopyButt
        if (other.name == "APR_Head" && name == "Copy Machine")
        {
            OfficeTaskManager.TaskCompleted("CopyButt");
        }
        #endregion

    }
    public void OnTriggerExit(Collider other)
    {
        #region MakeCoffee
        if (other == OfficeTaskManager.coffeeObject1 && name == "CoffeMaker")
        {
            OfficeTaskManager.coffeePartsCollected--;
        }
        if (other == OfficeTaskManager.coffeeObject2 && name == "CoffeMaker")
        {
            OfficeTaskManager.coffeePartsCollected--;
        }
        if (other == OfficeTaskManager.coffeeObject3 && name == "CoffeMaker")
        {
            OfficeTaskManager.coffeePartsCollected--;
        }
        #endregion
    }

}
