using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private void Awake()
    {
        aimanager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCount = 0;
        lNum = 0;
        CreateAllAI();
        player = FindObjectOfType<APRController>().Root.transform;

        Debug.Log("Number mesh COlliders: " + FindObjectsOfType<MeshCollider>().Length);
    }

    // Update is called once per frame
    void Update()
    {
        //DistanceCheck();
    }

    void DistanceCheck()
    {
        if (player == null)
        {
            player = FindObjectOfType<APRController>().Root.transform;

        }
        else
        {

            for(int i = 0; i < 20; i++)
            {
                if(lNum == 0)
                {
                    if(currentCount < people.Count)
                    {
                        var a = people[currentCount];
                        if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                        {
                            if (a.anim != null)
                            {
                               // a.anim.StopPlayback();
                              //  a.anim.enabled = false;
                            }
                            a.offByDistance = true;
                            a.disableAgent();
                            
                        }
                        else if (a.offByDistance)
                        {
                            a.enableAgent();
                            a.offByDistance = false;
                            if(a.anim != null)
                            {
                               // a.anim.enabled = true;
                               // a.anim.StartPlayback();
                            }
                        }
                        currentCount++;
                        if(currentCount >= people.Count)
                        {
                            currentCount = 0;
                            lNum++;
                        }
                    }
                    else
                    {
                        currentCount = 0;
                        lNum++;
                    }
                    
                }
                else if(lNum == 1)
                {
                    if(currentCount < crowds.Count)
                    {
                        var a = crowds[currentCount];
                        if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                        {
                            if (a.anim != null)
                            {
                                //a.anim.StopPlayback();
                               // a.anim.enabled = false;
                            }
                            a.offByDistance = true;
                            a.disableAgent();
                            foreach (var ld in a.AIMen)
                            {
                                if (ld.anim != null)
                                {
                                    //ld.anim.StopPlayback();
                                   // ld.anim.enabled = false;
                                }
                                ld.offByDistance = true;
                                ld.disableAgent(); ;
                                
                            }
                        }
                        else if (a.offByDistance)
                        {
                            a.enableAgent();
                            a.offByDistance = false;
                            if (a.anim != null)
                            {
                                //a.anim.enabled = true;
                               // a.anim.StartPlayback();
                            }
                            foreach (var ld in a.AIMen)
                            {
                                ld.offByDistance = false;
                                ld.enableAgent();
                                if (ld.anim != null)
                                {
                                   // ld.anim.enabled = true;
                                   // ld.anim.StartPlayback();
                                }
                            }
                        }

                        currentCount++;
                        if(currentCount >= crowds.Count)
                        {
                            currentCount = 0;
                            lNum++;
                        }
                    }
                    else
                    {
                        currentCount = 0;
                        lNum++;
                    }
                    
                }
                else if(lNum == 2)
                {
                    if(currentCount < stillPeople.Count)
                    {

                        var a = stillPeople[currentCount];
                        if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                        {
                            if (a.anim != null)
                            {
                                //a.anim.StopPlayback();
                                //a.anim.enabled = false;
                            }
                            a.offByDistance = true;
                            a.disableAgent();

                        }
                        else if (a.offByDistance)
                        {
                            a.enableAgent();
                            a.offByDistance = false;
                            if (a.anim != null)
                            {
                                //a.anim.enabled = true;
                                //a.anim.StartPlayback();
                            }
                        }

                        currentCount++;
                        if(currentCount >= stillPeople.Count)
                        {
                            currentCount = 0;
                            lNum++;
                        }
                    }
                    else
                    {
                        currentCount = 0;
                        lNum++;
                    }
                }
                else if(lNum == 3)
                {
                    if(currentCount < hostilePeople.Count)
                    {
                        var a = hostilePeople[currentCount];
                        if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                        {
                            if (a.anim != null)
                            {
                                //a.anim.StopPlayback();
                                //a.anim.enabled = false;
                            }
                            a.offByDistance = true;
                            a.disableAgent();
                        }
                        else if (a.offByDistance)
                        {
                            a.enableAgent();
                            a.offByDistance = false;
                            if (a.anim != null)
                            {
                                //a.anim.enabled = true;
                                //a.anim.StartPlayback();
                            }
                        }

                        currentCount++;
                        if(currentCount >= hostilePeople.Count)
                        {
                            currentCount = 0;
                            lNum++;
                        }
                    }
                    else
                    {
                        currentCount = 0;
                        lNum++;
                    }
                }
                else if(lNum == 4)
                {
                    if(currentCount < sitdownPeople.Count)
                    {
                        var a = sitdownPeople[currentCount];
                        if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                        {
                            if (a.anim != null)
                            {
                                //a.anim.StopPlayback();
                               // a.anim.enabled = false;
                            }
                            a.offByDistance = true;
                            a.disableAgent();
                        }
                        else if (a.offByDistance)
                        {
                            a.enableAgent();
                            a.offByDistance = false;
                            if (a.anim != null)
                            {
                                //a.anim.enabled = true;
                                //a.anim.StartPlayback();
                            }
                        }
                        currentCount++;
                        if(currentCount >= sitdownPeople.Count)
                        {
                            lNum = 0;
                            currentCount = 0;
                        }

                    }
                }
            }



            /*

            foreach (var a in people)
            {
                if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                {
                    a.offByDistance = true;
                    a.agent.enabled = false;
                }
                else if (a.offByDistance)
                {
                    a.agent.enabled = true;
                    a.offByDistance = false;
                }
            }
            foreach (var a in crowds)
            {
                if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                {
                    a.offByDistance = true;
                    a.agent.enabled = false;
                    foreach(var ld in a.AIMen)
                    {
                        ld.offByDistance = true;
                        ld.agent.enabled = false;
                    }
                }
                else if (a.offByDistance)
                {
                    a.agent.enabled = true;
                    a.offByDistance = false;
                    foreach (var ld in a.AIMen)
                    {
                        ld.offByDistance = false;
                        ld.agent.enabled = true ;
                    }
                }
            }
            foreach (var a in stillPeople)
            {
                if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                {
                    a.offByDistance = true;
                    a.agent.enabled = false;
                }
                else if (a.offByDistance)
                {
                    a.agent.enabled = true;
                    a.offByDistance = false;
                }
            }
            foreach (var a in hostilePeople)
            {
                if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                {
                    a.offByDistance = true;
                    a.agent.enabled = false;
                }
                else if (a.offByDistance)
                {
                    a.agent.enabled = true;
                    a.offByDistance = false;
                }
            }
            foreach (var a in sitdownPeople)
            {
                if (Vector3.Distance(a.transform.position, player.position) >= turnOffDistance)
                {
                    a.offByDistance = true;
                    a.agent.enabled = false;
                }
                else if (a.offByDistance)
                {
                    a.agent.enabled = true;
                    a.offByDistance = false;
                }
            }
            */
        }
    }


    void CreateAllAI()
    {
        foreach(var rd in RegularDudes)
        {
            people.Add(rd.MakeAI(normalAI));
        }
        foreach(var cd in CrowdDudes)
        {
            crowds.Add(cd.MakeAI(crowdAI) as CrowdAI);
        }
        foreach(var sa in StandStillDudes)
        {
            stillPeople.Add(sa.MakeAI(stillAI) as StayStillAIMan);
        }
        foreach(var hd in HostileDudes)
        {
            hostilePeople.Add(hd.MakeAI(hostileAI) as HostileAI);
        }
        foreach(var sd in SitDownDudes)
        {
            sitdownPeople.Add(sd.MakeAI(sitDownAI) as SitDownAI);
        }
    }





    [Header("Set AI Information here")]
    [SerializeField]
    public List<AIHolder> RegularDudes;
    [SerializeField]
    List<CrowdAIHolder> CrowdDudes;
    [SerializeField]
    public List<StillAIHolder> StandStillDudes;
    [SerializeField]
    public List<HostileAIHolder> HostileDudes;
    [SerializeField]
    public List<SitDownAIHolder> SitDownDudes;


    //[Header("Game Objects instantiated")]
    [HideInInspector]
    public List<NewAIMan> people;
    [HideInInspector]
    public List<CrowdAI> crowds;
    [HideInInspector]
    public List<StayStillAIMan> stillPeople;
    [HideInInspector]
    public List<HostileAI> hostilePeople;
    [HideInInspector]
    public List<SitDownAI> sitdownPeople;

    [Header("AI Prefabs")]
    public GameObject normalAI;
    public GameObject crowdAI;
    public GameObject stillAI;
    public GameObject hostileAI;
    public GameObject sitDownAI;

    [Header("Unity Information")]
    public Transform player;
    public float turnOffDistance = 50;
    [SerializeField]
    public List<GameObject> IgnorePhysicsIgnoring;

    int currentCount = 0;
    int lNum = 0;

    [HideInInspector]
    public static AIManager aimanager;
}
