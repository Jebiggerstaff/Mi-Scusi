using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CruiseShipTaskColider : MonoBehaviour
{
    CruiseShipTaskManager cruiseShipTaskManager = new CruiseShipTaskManager();


    public void Start()
    {
        cruiseShipTaskManager = GameObject.Find("TaskUI").GetComponent<CruiseShipTaskManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        #region EvacuateShip
        if (other.GetComponent<NewAIMan>() != null && name == "Water")
        {
            cruiseShipTaskManager.MenThrownInWater++;
            if (cruiseShipTaskManager.MenThrownInWater == 10)
            {
                cruiseShipTaskManager.TaskCompleted("EvacuateShip");
            }      
        }
        #endregion
        #region Slide
        if (other.name == "APR_Head" && name == "SlideColiders")
        {
            cruiseShipTaskManager.TaskCompleted("Waterslide");
        }
        #endregion
        #region Mutiny
        if(other.gameObject.name == "Captain")
        {
            cruiseShipTaskManager.TaskCompleted("Mutiny");
        }
        #endregion

    }
}
