﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(APRController))]
public class CharacterModelLoader : MonoBehaviour
{
    public GameObject Ass, Backpack, Coat, Hat, Legs, Shirt;

    public Mesh[] Hats;

    [Space]
    public Mesh[] Coats;

    [Space]
    public Mesh[] Shirts;

    [Space]
    public Mesh[] Accessories;

    [Space]
    public Mesh[] Pants;

    [Space]
    public Mesh[] Backpacks;

    private int hatModel;
    private int shirtModel;
    private int coatModel;
    private int pantsModel;
    private int accessoryModel;
    private int backpackModel;

    private void Awake()
    {
        GetInitialDefaults();
        LoadAllModels();
    }

    public void GetInitialDefaults()
    {
        if (PlayerPrefs.HasKey("Costume_Hat"))
        {
            hatModel = PlayerPrefs.GetInt("Costume_Hat");
        }
        else
        {
            hatModel = 0;
            PlayerPrefs.SetInt("Costume_Hat", 0);
        }
        if (PlayerPrefs.HasKey("Costume_Shirt"))
        {
            shirtModel = PlayerPrefs.GetInt("Costume_Shirt");
        }
        else
        {
            shirtModel = 0;
            PlayerPrefs.SetInt("Costume_Shirt", 0);
        }
        if (PlayerPrefs.HasKey("Costume_Coat"))
        {
            coatModel = PlayerPrefs.GetInt("Costume_Coat");
        }
        else
        {
            coatModel = 0;
            PlayerPrefs.SetInt("Costume_Coat", 0);
        }
        if (PlayerPrefs.HasKey("Costume_Pants"))
        {
            pantsModel = PlayerPrefs.GetInt("Costume_Pants");
        }
        else
        {
            pantsModel = 0;
            PlayerPrefs.SetInt("Costume_Pants", 0);
        }
        if (PlayerPrefs.HasKey("Costume_Backpack"))
        {
            backpackModel = PlayerPrefs.GetInt("Costume_Backpack");
        }
        else
        {
            backpackModel = 0;
            PlayerPrefs.SetInt("Costume_Backpack", 0);
        }
        if (PlayerPrefs.HasKey("Costume_Accessory"))
        {
            accessoryModel = PlayerPrefs.GetInt("Costume_Accessory");
        }
        else
        {
            accessoryModel = 0;
            PlayerPrefs.SetInt("Costume_Accessory", 0);
        }
    }

    public void LoadAllModels()
    {
        if(PlayerPrefs.HasKey("Costume_Hat"))
        {
            hatModel = PlayerPrefs.GetInt("Costume_Hat");
        }
        else
        {
            hatModel = 0;
        }
        if (PlayerPrefs.HasKey("Costume_Shirt"))
        {
            shirtModel = PlayerPrefs.GetInt("Costume_Shirt");
        }
        else
        {
            shirtModel = 0;
        }
        if (PlayerPrefs.HasKey("Costume_Coat"))
        {
            coatModel = PlayerPrefs.GetInt("Costume_Coat");
        }
        else
        {
            coatModel = 0;
        }
        if (PlayerPrefs.HasKey("Costume_Pants"))
        {
            pantsModel = PlayerPrefs.GetInt("Costume_Pants");
        }
        else
        {
            pantsModel = 0;
        }
        if (PlayerPrefs.HasKey("Costume_Backpack"))
        {
            backpackModel = PlayerPrefs.GetInt("Costume_Backpack");
        }
        else
        {
            backpackModel = 0;
        }
        if (PlayerPrefs.HasKey("Costume_Accessory"))
        {
            accessoryModel = PlayerPrefs.GetInt("Costume_Accessory");
        }
        else
        {
            accessoryModel = 0;
        }

        loadModelsFromInts();
    }

    private void loadModelsFromInts()
    {

        if (Hats.Length > 0)
            Hat.GetComponent<SkinnedMeshRenderer>().sharedMesh = Hats[hatModel];
            //Hats[hatModel].SetActive(true);
        if (Shirts.Length > 0)
            Shirt.GetComponent<SkinnedMeshRenderer>().sharedMesh = Shirts[shirtModel];
            //Shirts[shirtModel].SetActive(true);
        if (Coats.Length > 0)
            Coat.GetComponent<SkinnedMeshRenderer>().sharedMesh = Coats[coatModel];
            //Coats[coatModel].SetActive(true);
        if (Pants.Length > 0)
            Legs.GetComponent<SkinnedMeshRenderer>().sharedMesh = Pants[pantsModel];
            //Pants[pantsModel].SetActive(true);
        if (Backpacks.Length > 0)
            Backpack.GetComponent<SkinnedMeshRenderer>().sharedMesh = Backpacks[backpackModel];
            //Backpacks[backpackModel].SetActive(true);
        if (Accessories.Length > 0)
            Ass.GetComponent<SkinnedMeshRenderer>().sharedMesh = Accessories[accessoryModel];
            //Accessories[accessoryModel].SetActive(true);
        
    }
    
}
