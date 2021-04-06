using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipKitchenMove : MonoBehaviour
{
    StayStillAIMan aiman;

    // Start is called before the first frame update
    void Start()
    {
        aiman = GetComponent<StayStillAIMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(checkPlayerCostume())
        {
            Destroy(gameObject);
        }
    }


    bool checkPlayerCostume()
    {
        if(PlayerPrefs.GetInt("Costume_Hat", 0) == 10 && PlayerPrefs.GetInt("Costume_Coat", 0) == 1)
        {
            return true;
        }
        else
            return false;
    }
}
