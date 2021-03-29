using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineDoors : MonoBehaviour
{
    public GameObject MovingDoor;
    public int UpandDown = 1;
    public int LeftandRight = 0;
    public int ForwardandBack = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gold")
        {
            //dissapear.SetActive(true);
            MovingDoor.transform.Translate(LeftandRight, UpandDown, ForwardandBack);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gold")
        {
            //dissapear.SetActive(false);
            MovingDoor.transform.Translate(-LeftandRight, -UpandDown, -ForwardandBack);
        }
    }
}
