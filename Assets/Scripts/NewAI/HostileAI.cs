using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostileAI : NewAIMan
{

    public override void Start()
    {
        base.Start();

        player = FindObjectOfType<APRController>().Root.transform;

    }

    public override void Update()
    {
        base.Update();

        

        if(!offByDistance)
        {
            agent.acceleration = agent.speed * 4 / 3;

            if (isAggrod)
            {
                if (moveTowardsPlayerOnAggro)
                {
                    SetNewDestination(player.position);
                }
                if (Vector3.Distance(transform.position, player.position) <= punchRange)
                {
                    tryPunch();
                }

                aggroTime -= Time.deltaTime;
                currentMaximumAllowedAggroTime -= Time.deltaTime;

                if (aggroTime <= 0 || currentMaximumAllowedAggroTime <= 0 || FindObjectOfType<APRController>().currentHP <= 0 || hp <= 0)
                {
                    if (moveTowardsPlayerOnAggro)
                    {
                        forceNewDest();
                    }
                    isAggrod = false;

                }
            }
            currentPunchCD -= Time.deltaTime;



        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player_1") && shouldAggro())
        {
            AggroToPlayer();
        }

    }

    public void AggroToPlayer()
    {
        AggroToPlayer(chaseDuration);
    }
    public void AggroToPlayer(float time)
    {
        if(FindObjectOfType<APRController>().currentHP > 0)
        {
            if (!isAggrod)
            {
                currentMaximumAllowedAggroTime = maximumAllowedAggroTime;
            }
            isAggrod = true;
            aggroTime = time;
            quipped = false;
        }
    }
    void tryPunch()
    {
        if(currentPunchCD <= 0 && stunCount <= 0)
        {
            
            if (Mathf.Abs(Vector3.Angle(player.forward, transform.position - player.transform.position)) < angle)
            {
                currentPunchCD = punchCD;


                int rand = Random.Range(0, 2);
                if (rand == 0)
                    punchRight();
                else
                    punchLeft();




                Debug.Log("Punching");
            }
            
        }

    }



    void punchRight()
    {
        //UPDATE
        anim.SetBool("RightPunch", true);
        RightHand.punching = true;
        

        StartCoroutine(timeUnPunch(1));
    }
    void punchLeft()
    {
        anim.SetBool("LeftPunch", true);
        LeftHand.punching = true;

        StartCoroutine(timeUnPunch(1));

    }

    IEnumerator timeUnPunch(float animTime)
    {
        float counter = 0;
        while(counter < animTime)
        {
            counter += Time.deltaTime * anim.speed;
            yield return null;
        }
        unPunch();
        anim.SetBool("LeftPunch", false);
        anim.SetBool("RightPunch", false);
    }

    public override void unPunch()
    {
        RightHand.punching = false;
        LeftHand.punching = false;
    }

    bool shouldAggro()
    {
        if(SceneManager.GetActiveScene().name == "Italy")
        {
            if(playerWearingMafiaSkin())
            {
                return false;
            }
        }


        return true;
    }
    bool playerWearingMafiaSkin()
    {
        return (PlayerPrefs.GetInt("Costume_Hat", 0) == 13 && PlayerPrefs.GetInt("Costume_Coat", 0) == 4 && PlayerPrefs.GetInt("Costume_Shirt", 0) == 4 && PlayerPrefs.GetInt("Costume_Pants", 0) == 4) ;
    }


    public bool moveTowardsPlayerOnAggro;
    public float chaseDuration;
    public float maximumAllowedAggroTime;
    [Space]
    public float punchRange;
    public float punchCD;

    Transform player;
    public float angle = 45;

    [HideInInspector]
    public bool isAggrod;
    bool leashing;
    float aggroTime;
    float currentPunchCD;
    float currentMaximumAllowedAggroTime;

    public AIHandContact RightHand;
    public AIHandContact LeftHand;

    public bool isItalyTaskMan = false;
}
