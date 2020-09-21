using System.Collections;
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
    }



    [Header("Set AI Information here")]
    [SerializeField]
    public List<AIHolder> RegularDudes;
    [SerializeField]
    List<CrowdAIHolder> CrowdDudes;

    [Header("Game Objects instantiated")]
    public List<NewAIMan> people;
    public List<CrowdAI> crowds;

    [Header("AI Prefabs")]
    public GameObject normalAI;
    public GameObject crowdAI; 


}
