using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class CustomizationPopupBar : MonoBehaviour
{
    public Image bar;
    public Text text;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var t = EventSystem.current.currentSelectedGameObject.transform;
        var tmp = t.parent;

        bar.enabled = false;
        text.enabled = false;
        while (tmp != null)
        {
            if(tmp.name.Contains("Options") && tmp.name != "CustomOptionsBackground")
            {
                bar.enabled = true;
                text.enabled = true;
                text.text = t.gameObject.name;
                break;
            }
            tmp = tmp.parent;
        }
    }
}
