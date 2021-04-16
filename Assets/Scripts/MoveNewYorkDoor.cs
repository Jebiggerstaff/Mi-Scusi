using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNewYorkDoor : MonoBehaviour
{

    public GameObject door;
    public MeshRenderer btn;
    public Material pressedMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            Destroy(door);
            btn.material = pressedMat;
            Destroy(this);
            
        }
    }
}
