using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThankYouPeter : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed = 0f;
    public float jumpForce = 0f;
   


    [Header("Unity Stuff")]
    public float jumpingRaycastDistance;
    public Rigidbody rb;
    public float stairRaycastCheckRadius;

    //Internal Use Only
    bool grounded = true;
    float rotationCount = 0.2f;
    const float rotationResetCount = 0.2f;
    float oldVMove = -5;
    float oldHMove = -5;
    float notGroundedCount = 0;


    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
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
        if(oldHMove != HMovement || oldVMove != VMovement)
        {
            rotationCount = rotationResetCount;
        }
        if (VMovement != 0f || HMovement != 0f)
        {
            Vector3 Rotation = rb.velocity;
            Rotation.y = 0f;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Rotation);
            Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationCount);
            transform.rotation = newRotation;

            //adjust amount of rotation
            rotationCount = Mathf.Clamp(rotationCount + 0.01f, rotationResetCount, 1);
            oldHMove = HMovement;
            oldVMove = VMovement;
        }
        else
        {
            rotationCount = rotationResetCount;
        }

        //Jump
        if (controls.Player.Jump.triggered && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            notGroundedCount = 0;
        }
        if(!grounded)
        {
            if(notGroundedCount >= 1)
            {
                RaycastHit rhit;
                if (Physics.Raycast(transform.position, Vector3.down, out rhit, jumpingRaycastDistance, 1 << LayerMask.NameToLayer("Ground")) || Physics.Raycast(transform.position, Vector3.down, out rhit, jumpingRaycastDistance, 1 << LayerMask.NameToLayer("Stairs")))
                    grounded = true;
                notGroundedCount = 0;
            }
            else
            {
                notGroundedCount += Time.fixedDeltaTime;
            }
        }

        //Stair Detection
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, stairRaycastCheckRadius, Vector3.down, out hit, jumpingRaycastDistance, 1 << LayerMask.NameToLayer("Stairs")))
        {
            rb.AddForce(Vector3.up * rb.mass, ForceMode.Force);
        }

    }   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, jumpingRaycastDistance, 1 << LayerMask.NameToLayer("Ground")) || Physics.Raycast(transform.position, Vector3.down, out hit, jumpingRaycastDistance, 1 << LayerMask.NameToLayer("Stairs")))
                grounded = true;
        }
    }
}
