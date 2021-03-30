using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioCrowdAI : MonoBehaviour
{
    HostileAI ai;

    public bool beHostile;
    float count;
    public Transform Centerfield;
    int whatPunch;
    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<HostileAI>();
        whatPunch = Random.Range(0, 2);
        if(!beHostile)
        {
            ai.anim.SetBool("transitionOutOfPunch", false);

            if (whatPunch == 0)
            {

                ai.anim.SetBool("LeftPunch", true);
            }
            else
            {
                ai.anim.SetBool("RightPunch", true);

            }
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if(beHostile && !ai.isAggrod && count <= Random.Range(2f, 4f))
        {
            count = 0;
            ai.destinations[0] = Centerfield.position + new Vector3(Random.Range(0f, 25f), 0, Random.Range(0f, 25f));
        }
        if (!ai.anim.GetBool("transitionOutOfPunch"))
        {
           
        }

        if(!beHostile)
        {
            transform.LookAt(Centerfield);
        }
    }


    public void GoalMade()
    {
        if(!beHostile)
        {
            beHostile = true;
            ai.Explode(transform.position - (Centerfield.position - transform.position).normalized);
        }
        else
        {
            ai.anim.SetBool("transitionOutOfPunch", false);
        }
    }
}
