using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateAllAI();
        player = FindObjectOfType<APRController>().Root.transform;

        Debug.Log("Number mesh COlliders: " + FindObjectsOfType<MeshCollider>().Length);
    }

    // Update is called once per frame
    void Update()
    {
        DistanceCheck();
    }

    void DistanceCheck()
    {
        if (player == null)
        {
            player = FindObjectOfType<APRController>().Root.transform;

        }
        else
        {
            Debug.DrawRay(player.position, new Vector3(turnOffDistance, 0, 0), Color.red, Time.deltaTime * 1.1f);

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

}
