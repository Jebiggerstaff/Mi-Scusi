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
            aiman.target = new Vector3(-150.509995f, 181.869995f, 172.75f);
            aiman.shovesPlayer = false;
        }
    }


    bool checkPlayerCostume()
    {
        //TODO: CHECK PLAYER OUTFIT


        return false;
    }
}
