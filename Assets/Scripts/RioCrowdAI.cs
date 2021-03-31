using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioCrowdAI : MonoBehaviour
{
    HostileAI ai;
    
    float count;
    public Transform Centerfield;
    public bool isSoccerPlayer = false;


    bool goalmade = false;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<HostileAI>();

        ai.destinations = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {

        if(goalmade || isSoccerPlayer)
        {
            count += Time.deltaTime;
            if (!ai.isAggrod && count >= Random.Range(2f, 4f))
            {

                count = 0;


                if (ai.destinations.Count != 1)
                {
                    ai.destinations.Clear();
                    ai.destinations.Add(Centerfield.position + new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f)));
                }
                else
                    ai.destinations[0] = Centerfield.position + new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));

                ai.forceNewDest();
                
            }
            
        }
    }


    public void GoalMade()
    {
        if(!isSoccerPlayer)
        {
            ai.destinations = new List<Vector3>();
            ai.anim.SetBool("transitionOutOfPunch", false);
            int raand = Random.Range(0, 2);
            if (raand == 0)
            {
                ai.anim.SetBool("RightPunch", true);
            }
            else
            {

                ai.anim.SetBool("LeftPunch", true);
            }

            ai.animSpeedCap = Random.Range(0.5f, 1f);
            ai.Explode(transform.position - (Centerfield.position - transform.position).normalized);
            ai.Explode(transform.position - (Centerfield.position - transform.position).normalized);
            goalmade = true;
            ai.destinations.Clear();
            ai.destinations.Add(Centerfield.position + new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f)));
            ai.forceNewDest();
        }
        else
        {
            ai.anim.SetBool("transitionOutOfPunch", false);
            int raand = Random.Range(0, 2);
            if (raand == 0)
            {
                ai.anim.SetBool("RightPunch", true);
            }
            else
            {

                ai.anim.SetBool("LeftPunch", true);
            }
        }
        
    }
}
