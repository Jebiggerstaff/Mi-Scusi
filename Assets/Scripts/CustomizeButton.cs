using SpeedTutorMainMenuSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeButton : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHat(int costumeNumber)
    {
        if (image.color == Color.white)
            FindObjectOfType<MenuController>().SetHat(costumeNumber);
    }
    public void setShirt(int costumeNumber)
    {
        if (image.color == Color.white)
            FindObjectOfType<MenuController>().SetShirt(costumeNumber);
    }
    public void setCoat(int costumeNumber)
    {
        if (image.color == Color.white)
            FindObjectOfType<MenuController>().SetCoat(costumeNumber);
    }
    public void setPants(int costumeNumber)
    {
        if (image.color == Color.white)
            FindObjectOfType<MenuController>().SetPants(costumeNumber);
    }
    public void setAcc(int costumeNumber)
    {
        if (image.color == Color.white)
            FindObjectOfType<MenuController>().SetAccessory(costumeNumber);
    }
    public void setBackpacl(int costumeNumber)
    {
        if (image.color == Color.white)
            FindObjectOfType<MenuController>().SetBackpack(costumeNumber);
    }
}
