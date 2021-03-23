using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoxDirection : MonoBehaviour
{
    public List<WindBox> boxes;
    public bool goUp;


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
            foreach(var b in boxes)
            {
                b.SetDirection(goUp);
            }
        }
    }
}
