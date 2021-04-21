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
            PlayerPrefs.Save();
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

            GameObject.Find("FinalPlayer").GetComponent<APRController>().skinUnlockNotification();
            StartCoroutine(GameObject.Find("FinalPlayer").GetComponent<APRController>().MildBobbySauce());
        }

        Debug.Log(PlayerPrefs.GetString(CosmeticSaveName, ""));

        #region Hats
        
        if (name == "Chef's Hat") Hat(0);
        if (name == "Cop") Hat(1);
        if (name == "Crown") Hat(2);
        if (name == "Desert Hood") Hat(3);
        if (name == "Doom Helm") Hat(4);
        if (name == "Fedora") Hat(5);
        if (name == "Captain's Hat") Hat(6);
        if (name == "Robot") Hat(7);
        if (name == "Rubicks Cube") Hat(8);
        if (name == "Tall Chef's Hat") Hat(9);
        if (name == "Wizard Hat") Hat(10);
        if (name == "Treasure") Hat(11);
        if (name == "Helmet") Hat(12);
        if (name == "Top Hat") Hat(13);
        if (name == "Horse Head") Hat(14);
        if (name == "Alien Head") Hat(15);
        if (name == "Lizard Head") Hat(16);
        if (name == "Noodle") Hat(17);
        if (name == "Poofy Hair") Hat(18);
        if (name == "Rain Hood") Hat(19);
        if (name == "TV") Hat(20);
        if (name == "Shane's Head") Hat(21);
        if (name == "Jason's Head") Hat(22);
        if (name == "Alex's Head") Hat(23);
        if (name == "Jackson's Head") Hat(24);
        if (name == "James's Head") Hat(25);
        if (name == "Charlie's Head") Hat(26);
        if (name == "Isaac's Head") Hat(27);
        if (name == "Zach's Head") Hat(28);
        #endregion

        #region Coats
        if (name == "Captain's Jacket") Coat(0);
        if (name == "Chef Coat") Coat(1);
        if (name == "Cowboy Vest") Coat(2);
        if (name == "Puffy Coat") Coat(3);
        if (name == "Trench Coat") Coat(4);
        if (name == "Mafia Coat") Coat(5);
        if (name == "Fur Coat") Coat(6);
        if (name == "Rain Coat") Coat(7);
        if (name == "Plate Armor") Coat(8);
        if (name == "Jason's Coat") Coat(9);
        if (name == "Alex's Coat") Coat(10);
        if (name == "Jackson's Coat") Coat(11);
        if (name == "James's Coat") Coat(12);

        #endregion

        #region Shirts
        if (name == "Desert Tank") Shirt(0);
        if (name == "Generic Shirt") Shirt(1);
        if (name == "Tank Top") Shirt(2);
        if (name == "Mafia Shirt") Shirt(3);
        if (name == "Cop Shirt") Shirt(4);
        if (name == "Football Shirt") Shirt(5);
        if (name == "Shane's Shirt") Shirt(6);
        if (name == "Jason's Shirt") Shirt(7);
        if (name == "Charlie's Shirt") Shirt(8);
        if (name == "Jackson's Shirt") Shirt(9);
        if (name == "Zach's Shirt") Shirt(10);
        #endregion

        #region Pants
        if (name == "Jeans") Pant(0);
        if (name == "Tan Pants") Pant(1);
        if (name == "Tan Shorts") Pant(2);
        if (name == "Cowboy Boots") Pant(3);
        if (name == "Mafia Pants") Pant(4);
        if (name == "Cop Pants") Pant(5);
        if (name == "Plate Legguards") Pant(6);
        if (name == "Football Pants") Pant(7);
        if (name == "Shane's Pants") Pant(8);
        if (name == "Jason's Pants") Pant(9);
        if (name == "Alex's Pants") Pant(10);
        if (name == "Zach's Pants") Pant(11);
        #endregion

        #region Backpacks
        if (name == "Expedition") Backpack(0);
        if (name == "Generic") Backpack(1);
        if (name == "Guitar") Backpack(2);
        if (name == "Shield") Backpack(3);
        if (name == "Sword") Backpack(4);
        if (name == "Tentacles") Backpack(5);
        if (name == "Wings") Backpack(6);
        if (name == "Graffiti Pack") Backpack(7);
        if (name == "Cat") Backpack(8);
        #endregion

        #region Accessories
        if (name == "Mustache") Accessory(0);
        if (name == "Pipe") Accessory(1);
        if (name == "Sunglasses") Accessory(2);
        if (name == "Flower") Accessory(6);
        if (name == "Book") Accessory(3);
        if (name == "Satchel") Accessory(4);
        if (name == "Graffiti Bag") Accessory(5);
        if (name == "Headphones") Accessory(7);
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
        HatQuestionMarks[i].SetActive(false);
    }



}


