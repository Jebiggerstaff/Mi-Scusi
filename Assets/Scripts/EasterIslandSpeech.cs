using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterIslandSpeech : MonoBehaviour
{
    public ImageEffect compareTarget;
    public GameObject speechStuff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speechStuff.SetActive(compareTarget.enabled);
    }
}
