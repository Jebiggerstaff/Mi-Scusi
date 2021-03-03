using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSoakerOnOff : MonoBehaviour
{
    [HideInInspector]
    public bool isGrabbed;
    [HideInInspector]
    public int handCount;

    public GameObject stream;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrabbed = handCount > 0;
        stream.SetActive(isGrabbed);
    }


    
}
