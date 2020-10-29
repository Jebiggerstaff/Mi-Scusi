using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAggroVisual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(ai.isAggrod)
        {
            if(visual.activeSelf == false)
            {
                visual.SetActive(true);
            }
            transform.LookAt(Camera.main.transform);
        }
        else
        {
            if(visual.activeSelf == true)
            {
                visual.SetActive(false);
            }
        }
    }

    public HostileAI ai;
    public GameObject visual;
}
