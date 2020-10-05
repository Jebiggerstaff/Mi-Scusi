using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeWindowSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject hatOpt;
    public GameObject CoatOpt;
    public GameObject ShirtOpt;
    public GameObject PantsOpt;
    public GameObject BackpackOpt;
    public GameObject AccessoryOpt;

    public void TurnOnWindow(string window)
    {
        hatOpt.SetActive(false);
        CoatOpt.SetActive(false);
        ShirtOpt.SetActive(false);
        PantsOpt.SetActive(false);
        BackpackOpt.SetActive(false);
        AccessoryOpt.SetActive(false);

        if(window == "Hat")
        {
            hatOpt.SetActive(true);
        }
        if (window == "Pants")
        {
            PantsOpt.SetActive(true);
        }
        if (window == "Coat")
        {
            CoatOpt.SetActive(true);
        }
        if (window == "Shirt")
        {
            ShirtOpt.SetActive(true);
        }
        if (window == "Backpack")
        {
            BackpackOpt.SetActive(true);
        }
        if (window == "Accessory")
        {
            AccessoryOpt.SetActive(true);
        }

    }

}
