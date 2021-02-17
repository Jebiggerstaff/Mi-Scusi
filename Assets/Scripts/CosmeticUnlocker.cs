using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticUnlocker : MonoBehaviour
{

    public GameObject Fedora;

    public void UnlockOutfit(string name)
    {
        Debug.LogWarning("Unlock " + name);

        if (name == "Fedora")
        {
            Fedora.GetComponent<Image>().color = Color.white;
            Fedora.GetComponent<Button>().interactable = true;
        }
    }
}


