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
    public Transform Camera;

    //Internal Use Only
    bool grounded = true;
    float rotationCount = 0.2f;
    const float rotationResetCount = 0.2f;
    float notGroundedCount = 0;

    Vector2 Movement;
    Vector2 OldMovement=new Vector2(-5,-5);

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

    private void Update()
    {
        Jump();
    }

    void PlayerMovement()
    {

        #region Movement
        Movement.y = controls.Player.MoveY.ReadValue<float>();
        Movement.x = controls.Player.MoveX.ReadValue<float>();

        Vector3 Direction = Camera.rotation * new Vector3(Movement.x, 0f, Movement.y).normalized;
        Direction.y = 0f;
        Direction = Direction * speed + new Vector3(0, rb.velocity.y, 0);
        rb.velocity = Vector3.Lerp(rb.velocity, Direction, .8f);
        #endregion

        #region Rotation
        if (OldMovement.x != Movement.x || OldMovement.y != Movement.y)
        {
            rotationCount = rotationResetCount;
        }
        if (Movement.y != 0f || Movement.x != 0f)
        {
            Vector3 Rotation = rb.velocity;
            Rotation.y = 0f;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Rotation);
            Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationCount);
            transform.rotation = newRotation;

            //adjust amount of rotation
            rotationCount = Mathf.Clamp(rotationCount + 0.01f, rotationResetCount, 1);
            OldMovement= Movement;
        }
        else
        {
            rotationCount = rotationResetCount;
        }
        #endregion

        #region Stair Detection
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, stairRaycastCheckRadius, Vector3.down, out hit, jumpingRaycastDistance, 1 << LayerMask.NameToLayer("Stairs")))
        {
            rb.AddForce(Vector3.up * rb.mass, ForceMode.Force);
        }
        #endregion

    }

    public void Jump()
    {
        #region Jump
        if (controls.Player.Jump.triggered && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            notGroundedCount = 0;
        }
        if (!grounded)
        {
            if (notGroundedCount >= 1)
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
        #endregion
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
