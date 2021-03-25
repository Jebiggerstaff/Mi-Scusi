using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBuilding : MonoBehaviour
{
    public GameObject IntactBuilding;
    public GameObject BrokenBuilding;
    public GameObject[] BrokenPieces;
    public Transform explosionPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        IntactBuilding.SetActive(false);
        BrokenBuilding.SetActive(true);
        foreach(var go in BrokenPieces)
        {
            go.GetComponent<Rigidbody>().AddExplosionForce(5000, explosionPoint.transform.position, 50, 3);
        }
    }
}
