using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThankYouPeter : MonoBehaviour
{
    public float speed = 0f;
    public float jumpForce = 0f;
    Rigidbody rb;
    bool grounded=true;

    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {     
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //Movement
        float VMovement = controls.Player.MoveY.ReadValue<float>();
        float HMovement = controls.Player.MoveX.ReadValue<float>();

        Vector3 Direction = new Vector3(HMovement,0f,VMovement).normalized;
        Direction = Direction * speed + new Vector3(0, rb.velocity.y, 0);
        rb.velocity = Vector3.Lerp(rb.velocity, Direction, .8f);

        //Rotation
        if (VMovement != 0f || HMovement != 0f)
        {
            Vector3 Rotation = rb.velocity;
            Rotation.y = 0f;
            transform.rotation = Quaternion.LookRotation(Rotation);
        }

        //Jump
        if (controls.Player.Jump.triggered && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
        }
    }   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, 1 << LayerMask.NameToLayer("Ground")))
                grounded = true;
        }
    }
}
