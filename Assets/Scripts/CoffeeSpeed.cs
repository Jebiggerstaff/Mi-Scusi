using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeSpeed : MonoBehaviour
{
    public GameObject coffeeparticles;
    GameObject instantiatedCoffee;

    private void Start()
    {
        instantiatedCoffee = null;
    }

    private void Update()
    {
        if(Time.timeScale > 1.0f)
        {
            Debug.Log(Time.timeScale);
            Time.timeScale = Time.timeScale - .001f;
        }
        if(Time.timeScale<1.01f && Time.timeScale > 1f)
        {
            Time.timeScale = 1f;
            if (instantiatedCoffee != null)
                Destroy(instantiatedCoffee);
        }
    }

    public void SpeedUp()
    {
        if(instantiatedCoffee == null)
            instantiatedCoffee = Instantiate(coffeeparticles, FindObjectOfType<APRController>().Root.transform);
        
        Debug.Log("COFEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
        Time.timeScale = 1.0f;
        if (Time.timeScale == 1.0f)
            Time.timeScale = 2f;
    }
}

