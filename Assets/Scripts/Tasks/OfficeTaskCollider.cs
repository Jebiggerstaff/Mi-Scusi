using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTaskCollider : MonoBehaviour
{

    public OfficeTaskManager OfficeTaskManager;

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
        #endregion
        //Code to talk to the Receptionist
        //probably needs to be done in the talking script
        #region CoffeeToBoss
        if (other.name== "CoffeeMugBoss" && name == "Boss")
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

}
