using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertCactusEffect : MonoBehaviour
{
    public ImageEffect cactusEffect;
    float cactusTimer;
    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cactusTimer >= 0)
        {

            cactusTimer -= Time.deltaTime;
            cactusEffect.enabled = true;
            if(Time.timeScale == 1)
            {

                Time.timeScale = 2;

            }
            isOn = true;
        }
        else
        {
            if(isOn)
            {
                isOn = false;
                Time.timeScale = 1;
                cactusEffect.enabled = false;
            }
        }
    }


    public void ActivateCactus()
    {
        cactusTimer = 30f;
    }
}
