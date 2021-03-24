using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{

    bool launching;
    bool flying;
    public float launchTime;
    float currentLaunchTime;
    public float flightTime;
    float currentFlightTime;
    public float baseFlightSpeed;
    float currentFlightSpeed;
    public float flightAcceleration;
    public float explosionRadius;
    Vector3 direction;
    Rigidbody rb;

    public GameObject explosionParticles;
    public ParticleSystem flightParticles;
    public ParticleSystem launchParticles;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFlightTime = 0;
        currentLaunchTime = 0;
        currentFlightSpeed = baseFlightSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(launching)
        {
            currentLaunchTime += Time.deltaTime;
            if(currentLaunchTime >= launchTime)
            {
                BeginFlight();
            }

        }

        
    }
    private void FixedUpdate()
    {
        if (flying)
        {
            currentFlightTime += Time.fixedDeltaTime;

            if (currentFlightTime >= flightTime)
            {
                Explode();
            }
            direction = transform.forward;
            transform.LookAt(transform.position + direction);
            rb.AddForce(direction * currentFlightSpeed * rb.mass);
            currentFlightSpeed += (flightAcceleration * Time.fixedDeltaTime);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HandContact>() != null)
        {
            BeginLaunch();

        }
        if(collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            rb.freezeRotation = false;
        }
    }


    void BeginLaunch()
    {
        if(!launching && !flying)
        {

            launching = true;
            launchParticles.Play();
        }
    }
    void BeginFlight()
    {
        if(!flying)
        {
            launchParticles.Stop();
            flightParticles.Play();
            launching = false;
            flying = true;
            direction = transform.up;

        }
    }
    void Explode()
    {
        flightParticles.Stop();
        Instantiate(explosionParticles, transform.position, transform.rotation);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            
            if (rb != null)
                rb.AddExplosionForce(5000, explosionPos, explosionRadius, 3);
            if (hit.GetComponent<NewAIMan>() != null)
            {
                hit.GetComponent<NewAIMan>().Explode(transform.position);
            }
        }

        Destroy(gameObject);


    }
}
