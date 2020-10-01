using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(APRController))]
public class CharacterModelLoader : MonoBehaviour
{

    private void Awake()
    {
        GetInitialDefaults();
        LoadAllModels();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void TurnOffAll()
    {
        foreach(var m in Hats)
        {
            m.SetActive(false);
        }
        foreach (var m in Shirts)
        {
            m.SetActive(false);
        }
        foreach (var m in Coats)
        {
            m.SetActive(false);
        }
        foreach (var m in Pants)
        {
            m.SetActive(false);
        }
        foreach (var m in Backpacks)
        {
            m.SetActive(false);
        }
        foreach (var m in Accessories)
        {
            m.SetActive(false);
        }
    }


    private void loadModelsFromInts()
    {
        TurnOffAll();

        if (Hats.Length > 0)
            Hats[hatModel].SetActive(true);
        if (Shirts.Length > 0)
            Shirts[shirtModel].SetActive(true);
        if (Coats.Length > 0)
            Coats[coatModel].SetActive(true);
        if (Pants.Length > 0)
            Pants[pantsModel].SetActive(true);
        if (Backpacks.Length > 0)
            Backpacks[backpackModel].SetActive(true);
        if (Accessories.Length > 0)
            Accessories[accessoryModel].SetActive(true);
        
    }
    
    public GameObject[] Hats;

   
    [Space]
    public GameObject[] Coats;


    [Space]
    public GameObject[] Shirts;


    [Space]
    public GameObject[] Accessories;


    [Space]
    public GameObject[] Pants;


    [Space]
    public GameObject[] Backpacks;



    

    private int hatModel;
    private int shirtModel;
    private int coatModel;
    private int pantsModel;
    private int accessoryModel;
    private int backpackModel;


}
