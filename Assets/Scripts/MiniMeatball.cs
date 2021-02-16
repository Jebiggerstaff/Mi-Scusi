using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMeatball : MonoBehaviour
{
    static int numMeatballsCollected;

    // Start is called before the first frame update
    void Start()
    {
        numMeatballsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CauldronInterior")
        {
            numMeatballsCollected++;
            if (numMeatballsCollected >= 3)
            {
                FindObjectOfType<ItalyTaskManager>().TaskCompleted("GiantMeatball");
            }
            Destroy(gameObject);
        }
    }

}
