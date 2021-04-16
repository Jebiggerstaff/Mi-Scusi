using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonMac : MonoBehaviour
{
    public GameObject mac;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "").Contains("Penroni"))
        {
            mac.SetActive(true);
            mac.GetComponent<Rigidbody>().angularVelocity = 30 * (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
