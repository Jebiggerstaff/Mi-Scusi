using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    MiScusiActions controls;
    public GameObject spine, head, root;
    public float force;
    public Vector3 directionforce;

    // Start is called before the first frame update
    void Start()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }
    private void FixedUpdate()
    {
        if (controls.Player.Jump.ReadValue<float>() != 0)
        {
            directionforce = (head.transform.position - spine.transform.position).normalized;
            spine.GetComponent<Rigidbody>().velocity = directionforce * force;
        }
    }
    // Update is called once per frame
    /*void Update()
    {
        if (controls.Player.Jump.ReadValue<float>() != 0)
        {
            directionforce = spine.transform.localRotation.eulerAngles;
            spine.GetComponent<Rigidbody>().velocity = directionforce;
        }
    }*/
}
