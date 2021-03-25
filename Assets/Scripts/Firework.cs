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
    public float explosionForce = 1250f;

    public GameObject explosionParticles;
    public ParticleSystem flightParticles;
    public ParticleSystem launchParticles;
    public static int numSpawned = 0;

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


    public void BeginLaunch()
    {
        if(!launching && !flying)
        {

            launching = true;
            launchParticles.Play();
        }
    }
    public void BeginFlight()
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
    public void Explode()
    {
        flightParticles.Stop();

        if (numSpawned < 10)
        {

            Instantiate(explosionParticles, transform.position, transform.rotation);

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (hit.GetComponent<Firework>() != null)
                {
                    hit.GetComponent<Firework>().BeginFlight();
                    rb.freezeRotation = false;

                }
                if (rb != null)
                    rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 1);
                if (hit.GetComponent<NewAIMan>() != null)
                {
                    hit.GetComponent<NewAIMan>().Explode(transform.position);
                }
                if (hit.gameObject.name == "ShopThing")
                {
                    FindObjectOfType<DesertTaskManager>().ShopThingsMessedWith++;
                    if (FindObjectOfType<DesertTaskManager>().ShopThingsMessedWith >= 15)
                    {
                        Debug.Log("Done!");
                        FindObjectOfType<DesertTaskManager>().TaskCompleted("MessUpShop");
                    }
                }

            }


            FindObjectOfType<DesertTaskManager>().FireworkExploded();

        }

        if(gameObject.name == "ShopFirework")
        {
            FindObjectOfType<DesertTaskManager>().shopFireworks++;
            if ( FindObjectOfType<DesertTaskManager>().shopFireworks >= 6)
            {

                FindObjectOfType<DesertTaskManager>().TaskCompleted("Fireworks");
            }
        }


        Destroy(gameObject);


    }

    
    
}
