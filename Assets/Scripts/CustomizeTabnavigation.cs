using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeTabnavigation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hats.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "Hats";
            Navigation newNav = GetComponent<Button>().navigation;
            newNav.selectOnDown = Hat1.GetComponent<Button>();
            GetComponent<Button>().navigation = newNav;
        }
        if (coats.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "Coats";
            Navigation newNav = GetComponent<Button>().navigation;
            newNav.selectOnDown = Coat1.GetComponent<Button>();
            GetComponent<Button>().navigation = newNav;
        }
        if (shirts.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "Shirts";
            Navigation newNav = GetComponent<Button>().navigation;
            newNav.selectOnDown = Shirt1.GetComponent<Button>();
            GetComponent<Button>().navigation = newNav;
        }
        if (pants.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "Pants";
            Navigation newNav = GetComponent<Button>().navigation;
            newNav.selectOnDown = Pant1.GetComponent<Button>();
            GetComponent<Button>().navigation = newNav;
        }
        if (backpacks.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "Backpacks";
            Navigation newNav = GetComponent<Button>().navigation;
            newNav.selectOnDown = back1.GetComponent<Button>();
            GetComponent<Button>().navigation = newNav;
        }
        if (accessories.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "Accessories";
            Navigation newNav = GetComponent<Button>().navigation;
            newNav.selectOnDown = Accessory1.GetComponent<Button>();
            GetComponent<Button>().navigation = newNav;
        }
    }


    public GameObject Hat1;
    public GameObject Shirt1;
    public GameObject Coat1;
    public GameObject Pant1;
    public GameObject back1;
    public GameObject Accessory1;


    public GameObject hats;
    public GameObject coats;
    public GameObject shirts;
    public GameObject pants;
    public GameObject backpacks;
    public GameObject accessories;


}
