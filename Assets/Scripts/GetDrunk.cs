using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDrunk : MonoBehaviour
{
    float drunktime = 0.0f;
    public GameObject kegCactus;
    public GameObject pickaxeHandsRight;
    public GameObject pickaxeHandsLeft;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        drunktime -= Time.deltaTime;

        if (kegCactus.activeSelf == true)
        {
            drunktime = 30.0f;
            kegCactus.SetActive(false);
        }

        if (drunktime > 0)
        {
            pickaxeHandsLeft.SetActive(true);
            pickaxeHandsRight.SetActive(true);
        }

        if (drunktime <= 0)
        {
            pickaxeHandsLeft.SetActive(false);
            pickaxeHandsRight.SetActive(false);
        }
    }
}
