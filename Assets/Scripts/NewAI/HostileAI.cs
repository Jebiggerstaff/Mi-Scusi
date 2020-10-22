using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        if(isAggrod)
        {
            if(moveTowardsPlayerOnAggro)
            {
                SetNewDestination(player.position);
            }
            if(Vector3.Distance(transform.position, player.position) <= punchRange)
            {
                tryPunch();
            }

            aggroTime -= Time.deltaTime;
            currentMaximumAllowedAggroTime -= Time.deltaTime;

            if (aggroTime <= 0 || currentMaximumAllowedAggroTime <= 0 || FindObjectOfType<APRController>().currentHP <= 0)
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


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player_1"))
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
        }
    }
    void tryPunch()
    {
        if(currentPunchCD <= 0)
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



    void punchRight()
    {
        apr.AIPunch(true);
    }
    void punchLeft()
    {

        apr.AIPunch(false);
    }








    public bool moveTowardsPlayerOnAggro;
    public float chaseDuration;
    public float maximumAllowedAggroTime;
    [Space]
    public float punchRange;
    public float punchCD;

    Transform player;

    bool isAggrod;
    bool leashing;
    float aggroTime;
    float currentPunchCD;
    float currentMaximumAllowedAggroTime;

    public AIRagdollMan apr;
}
