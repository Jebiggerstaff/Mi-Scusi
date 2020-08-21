using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingpuff : MonoBehaviour
{

    public GameObject foamFrontWalk;
    public GameObject foamBackWalk;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            foamFrontWalk.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            foamFrontWalk.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            foamBackWalk.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            foamBackWalk.SetActive(false);
        }
    }
}
