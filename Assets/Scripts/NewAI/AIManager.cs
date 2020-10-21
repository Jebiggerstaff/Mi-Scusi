﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateAllAI();
    }

    // Update is called once per frame
    void Update()
    {
        
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


    //[Header("Game Objects instantiated")]
    [HideInInspector]
    public List<NewAIMan> people;
    [HideInInspector]
    public List<CrowdAI> crowds;
    [HideInInspector]
    public List<StayStillAIMan> stillPeople;
    [HideInInspector]
    public List<HostileAI> hostilePeople;

    [Header("AI Prefabs")]
    public GameObject normalAI;
    public GameObject crowdAI;
    public GameObject stillAI;
    public GameObject hostileAI;


}
