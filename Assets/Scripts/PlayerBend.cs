using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBend : MonoBehaviour
{
    private MiScusiActions controls;
    private GameObject[] PlayerBones;

    public GameObject lowerSpine, midSpine, upperSpine;

    private float MouseYAxisBody;
    private float bendAdd=0f;

    void Awake()
    {
        PlayerSetup();
        controls = new MiScusiActions();
        controls.Enable();
    }

    void Update()
    {
        playerspine();
    }

    void PlayerSetup()
    {
        //Setup/reroute active ragdoll parts to array
        PlayerBones = new GameObject[]
        {
			//0
			lowerSpine,
			//1
			midSpine,
			//2
			upperSpine
        };
    }

    void playerspine()
    {
        bendAdd -= .2f * controls.Player.Bend.ReadValue<float>();
        //Mathf.Clamp(bendAdd,-90,90);
        if (bendAdd > 90)
            bendAdd = 90;
        if (bendAdd < -90)
            bendAdd = -90;
        midSpine.transform.localRotation = Quaternion.Euler(bendAdd, 0f, 0f);       
        
    }
}
