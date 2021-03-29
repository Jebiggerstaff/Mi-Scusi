using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItalyMoveNoirGuards : MonoBehaviour
{
    StayStillAIMan AI;
    public Vector3 moveTarget;

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<StayStillAIMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(checkPlayerCostume())
        {
            if(AI == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                AI.target = moveTarget;
                AI.shovesPlayer = false;
            }
        }
    }




    bool checkPlayerCostume()
    {
        if (PlayerPrefs.GetInt("Costume_Hat", 0) == 2 && PlayerPrefs.GetInt("Costume_Shirt", 0) == 5 && PlayerPrefs.GetInt("Costume_Pants", 0) == 5)
            return true;

        return false;
    }
}
