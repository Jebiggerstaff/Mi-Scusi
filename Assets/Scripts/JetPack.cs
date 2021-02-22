using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    MiScusiActions controls;
    public GameObject spine;
    public int force;
    private Vector3 directionforce;

    // Start is called before the first frame update
    void Start()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.Player.Jump.)
        {
            directionforce = spine.;
            spine.GetComponent<Rigidbody>().velocity = directionforce;
        }
    }
}
