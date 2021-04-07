using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raygun : MonoBehaviour
{
    public GameObject Remains;
    public GameObject PoofEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEffects(Vector3 point)
    {
        Instantiate(Remains, point, Quaternion.Euler(0, 0, 0));
        Instantiate(PoofEffect, point, Quaternion.Euler(0, 0, 0));
    }
}
