using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationButtonLeftSet : MonoBehaviour
{
    public GameObject hat;
    public GameObject shirt;
    public GameObject pants;
    public GameObject coat;
    public GameObject backpack;
    public GameObject accessory;
    public GameObject defaultPath;
    [Space]
    public Button me;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var nav = me.navigation;
        nav.selectOnLeft = null;
        if(hat.activeSelf)
        {
            foreach(var b in hat.GetComponentsInChildren<Button>())
            {
                if(b.interactable)
                {
                    nav.selectOnLeft = b;
                    break;
                }
            }
        }
        if(shirt.activeSelf)
        {
            foreach (var b in shirt.GetComponentsInChildren<Button>())
            {
                if (b.interactable)
                {
                    nav.selectOnLeft = b;
                    break;
                }
            }
        }

        if (coat.activeSelf)
        {
            foreach (var b in coat.GetComponentsInChildren<Button>())
            {
                if (b.interactable)
                {
                    nav.selectOnLeft = b;
                    break;
                }
            }
        }

        if (pants.activeSelf)
        {
            foreach (var b in pants.GetComponentsInChildren<Button>())
            {
                if (b.interactable)
                {
                    nav.selectOnLeft = b;
                    break;
                }
            }
        }

        if (backpack.activeSelf)
        {
            foreach (var b in backpack.GetComponentsInChildren<Button>())
            {
                if (b.interactable)
                {
                    nav.selectOnLeft = b;
                    break;
                }
            }
        }

        if (accessory.activeSelf)
        {
            foreach (var b in accessory.GetComponentsInChildren<Button>())
            {
                if (b.interactable)
                {
                    nav.selectOnLeft = b;
                    break;
                }
            }
        }

        if (nav.selectOnLeft == null && defaultPath.activeSelf == true)
            nav.selectOnLeft = defaultPath.GetComponent<Button>();

        me.navigation = nav;
    }
}
