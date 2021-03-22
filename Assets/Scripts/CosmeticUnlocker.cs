using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticUnlocker : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public List<GameObject> Hats;
    [SerializeField]
    public List<GameObject> HatQuestionMarks;
    [SerializeField]
    public List<GameObject> Coats;
    [SerializeField]
    public List<GameObject> CoatQuestionMarks;
    [SerializeField]
    public List<GameObject> Shirts;
    [SerializeField]
    public List<GameObject> ShirtQuestionMarks;
    [SerializeField]
    public List<GameObject> Pants;
    [SerializeField]
    public List<GameObject> PantsQuestionMarks;
    [SerializeField]
    public List<GameObject> Backpacks;
    [SerializeField]
    public List<GameObject> BackpacksQuestionMarks;
    [SerializeField]
    public List<GameObject> Accessories;
    [SerializeField]
    public List<GameObject> AccessoryQuestionMarks;

    public static readonly string CosmeticSaveName = "CosmoLock";
    private static readonly string CosmeticBufferString = "&$&";

    #endregion

    private void Start()
    {
        string prefs = "";
        if(PlayerPrefs.HasKey(CosmeticSaveName))
        {
            prefs = PlayerPrefs.GetString(CosmeticSaveName, "");
        }
        else
        {
            PlayerPrefs.SetString(CosmeticSaveName, "");
        }
        foreach(var go in Hats)
        {
            if(prefs.Contains(CosmeticBufferString + go.name + CosmeticBufferString))
            {
                UnlockOutfit(go.name);
            }
        }
        foreach (var go in Shirts)
        {
            if (prefs.Contains(CosmeticBufferString + go.name + CosmeticBufferString))
            {
                UnlockOutfit(go.name);
            }
        }
        foreach (var go in Coats)
        {
            if (prefs.Contains(CosmeticBufferString + go.name + CosmeticBufferString))
            {
                UnlockOutfit(go.name);
            }
        }
        foreach (var go in Pants)
        {
            if (prefs.Contains(CosmeticBufferString + go.name + CosmeticBufferString))
            {
                UnlockOutfit(go.name);
            }
        }
        foreach (var go in Backpacks)
        {
            if (prefs.Contains(CosmeticBufferString + go.name + CosmeticBufferString))
            {
                UnlockOutfit(go.name);
            }
        }
        foreach (var go in Accessories)
        {
            if (prefs.Contains(CosmeticBufferString + go.name + CosmeticBufferString))
            {
                UnlockOutfit(go.name);
            }
        }
    }

    public void UnlockOutfit(string name)
    {
        Debug.LogWarning("Unlock " + name);


        string prefs = PlayerPrefs.GetString(CosmeticSaveName, "");
        if (prefs.Contains(CosmeticBufferString + name + CosmeticBufferString))
        {
            //you have it
        }
        else
        {
            PlayerPrefs.SetString(CosmeticSaveName, PlayerPrefs.GetString(CosmeticSaveName, "") + CosmeticBufferString + name + CosmeticBufferString);
            PlayerPrefs.Save();

            //TODO: SHOW COSMETIC UNLOCKED POPUP
        }

        Debug.Log(PlayerPrefs.GetString(CosmeticSaveName, ""));

        #region Hats
        if (name == "CaptainHat") Hat(0);
        if (name == "ChefItaly") Hat(1);
        if (name == "Cop") Hat(2);
        if (name == "Crown") Hat(3);
        if (name == "Desert") Hat(4);
        if (name == "DicesSet") Hat(5);
        if (name == "DoomGuy") Hat(6);
        if (name == "Fedora") Hat(7);
        if (name == "Flower") Hat(8);
        if (name == "Headphones") Hat(9);
        if (name == "RobotHead") Hat(10);
        if (name == "RubicksCube") Hat(11);
        if (name == "TallChefHat") Hat(12);
        if (name == "WizardHat") Hat(13);
        if (name == "Treasure") Hat(14);
        if (name == "ArmorHelmet") Hat(15);
        if (name == "TopHat") Hat(16);
        if (name == "HorseHead") Hat(17);
        if (name == "AlienHead") Hat(18);
        if (name == "LizardHead") Hat(19);
        if (name == "Noodle") Hat(20);
        if (name == "PoofHair") Hat(21);
        #endregion

        #region Coats
        if (name == "CaptainsJacket") Coat(0);
        if (name == "ChefCoat") Coat(1);
        if (name == "Cowboy") Coat(2);
        if (name == "PuffyCoat") Coat(3);
        if (name == "TrenchCoat") Coat(4);
        if (name == "MafiaCoat") Coat(5);
        if (name == "FurCoat") Coat(6);
        if (name == "RainCoat") Coat(7);
        if (name == "ArmorCoat") Coat(8);
        #endregion

        #region Shirts
        if (name == "DesertShirt") Shirt(0);
        if (name == "GenericShirt") Shirt(1);
        if (name == "TankTop") Shirt(2);
        if (name == "MafiaShirt") Shirt(3);
        if (name == "CopShirt") Shirt(4);
        #endregion

        #region Pants
        if (name == "Jeans") Pant(0);
        if (name == "Pants") Pant(1);
        if (name == "Shorts") Pant(2);
        if (name == "CowboyBoots") Pant(3);
        if (name == "MafiaPants") Pant(4);
        if (name == "CopPants") Pant(5);
        if (name == "ArmorPants") Pant(6);
        #endregion

        #region Backpacks
        if (name == "Expedition") Backpack(0);
        if (name == "GenericBP") Backpack(1);
        if (name == "Guitar") Backpack(2);
        if (name == "Shield") Backpack(3);
        if (name == "Sword") Backpack(4);
        if (name == "Tenticles") Backpack(5);
        if (name == "Wings") Backpack(6);
        if (name == "Graffiti") Backpack(7);
        if (name == "Cat") Backpack(8);
        #endregion

        #region Accessories
        if (name == "Mustache") Accessory(0);
        if (name == "Pipe") Accessory(1);
        if (name == "SunGlasses") Accessory(2);
        if (name == "Flower") Accessory(3);
        if (name == "Book") Accessory(4);
        if (name == "Satchel") Accessory(5);
        if (name == "GraffitiSachel") Accessory(6);
        #endregion

    }

    public void Accessory(int i)
    {
        Accessories[i].GetComponent<Image>().color = Color.white;
        Accessories[i].GetComponent<Button>().interactable = true;
        AccessoryQuestionMarks[i].SetActive(false);
    }

    public void Backpack(int i)
    {
        Backpacks[i].GetComponent<Image>().color = Color.white;
        Backpacks[i].GetComponent<Button>().interactable = true;
        BackpacksQuestionMarks[i].SetActive(false);
    }

    public void Pant(int i)
    {
        Pants[i].GetComponent<Image>().color = Color.white;
        Pants[i].GetComponent<Button>().interactable = true;
        PantsQuestionMarks[i].SetActive(false);
    }

    public void Shirt(int i)
    {
        Shirts[i].GetComponent<Image>().color = Color.white;
        Shirts[i].GetComponent<Button>().interactable = true;
        ShirtQuestionMarks[i].SetActive(false);
    }

    public void Coat(int i)
    {
        Coats[i].GetComponent<Image>().color = Color.white;
        Coats[i].GetComponent<Button>().interactable = true;
        CoatQuestionMarks[i].SetActive(false);
    }

    public void Hat(int i)
    {
        Hats[i].GetComponent<Image>().color = Color.white;
        Hats[i].GetComponent<Button>().interactable = true;
        HatQuestionMarks[i-1].SetActive(false);
    }

}


