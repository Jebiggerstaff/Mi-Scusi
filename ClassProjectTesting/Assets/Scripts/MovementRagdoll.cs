using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRagdoll : MonoBehaviour
{

    public GameObject camera1;
    public GameObject camera2;

    public Animator Anim;

    Rigidbody rb;
    CapsuleCollider Cap;
    float vertical;
    float horizontal;

    float verticalRaw;
    float horizontalRaw;

    Vector3 targetRotation;

    public float RotationSpeed = 10;
    public float speed = 100;
    public float JumpForce = 7;

    bool isGround = true;
    bool isGroundItem = true;

    public float Outvalue = 10;

    public float movementSpeed = 25.0f;

    public float timer = 0f;

    public GameObject SpyGlass;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cap = GetComponent<CapsuleCollider>();
    }

   void Update()
    {

        if(SpyGlass.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.C) && camera2.activeSelf == false)
            {
                camera1.SetActive(false);
                camera2.SetActive(true);
                movementSpeed = 0.0f;
                RotationSpeed = 0.0f;
            }

            else if (Input.GetKeyDown(KeyCode.C) && camera2.activeSelf == true)
            {
                camera1.SetActive(true);
                camera2.SetActive(false);
                movementSpeed = 25.0f;
                RotationSpeed = 10.0f;
            }
        }
   

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(new Vector3(0, JumpForce * 100, 0), ForceMode.Impulse);
            isGround = false;
        }

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movementSpeed * Time.deltaTime);

    
    }

        void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0, vertical);
        Vector3 inputRaw = new Vector3(horizontalRaw, 0, verticalRaw);

        rb.velocity = Vector3.zero;

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        if (inputRaw != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input).eulerAngles;
        }
        rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * RotationSpeed);

        if (inputRaw.sqrMagnitude != 0)
        {
            Anim.enabled = true;
        }
        else if (inputRaw.sqrMagnitude == 0)
        {
            Anim.enabled = false;
            rb.velocity = Vector3.zero;
        }

        if (isGround == false)
        {
            Anim.enabled = false;
           

        }
      
     

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > Outvalue)
        {
            StartCoroutine(Out());
        }

        if (other.gameObject.CompareTag("Plane"))
        {
            isGround = true;
            isGroundItem = true;
        }
        if (other.gameObject.CompareTag("Untagged"))
        {
            isGround = true;
            isGroundItem = true;
        }
        if (other.gameObject.CompareTag("Item") && isGroundItem == true)
        {
            isGround = true;
            isGroundItem = false;
        }

    }

    IEnumerator Out()
    {
        yield return new WaitForSeconds(2);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Cap.enabled = true;
    }
}
