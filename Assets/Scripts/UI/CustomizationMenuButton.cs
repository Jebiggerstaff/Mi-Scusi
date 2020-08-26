using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationMenuButton : MonoBehaviour
{
    
    private void OnMouseDown()
    {
        var mc = FindObjectOfType<SpeedTutorMainMenuSystem.MenuController>();
        if(mc != null)
        {
            switch(type)
            {
                case CustomizationType.Arms:
                    mc.SetArms(customizationName);
                    break;
                case CustomizationType.Legs:
                    mc.SetLegs(customizationName);
                    break;
                case CustomizationType.Head:
                    mc.SetHead(customizationName);
                    break;
                case CustomizationType.Body:
                    mc.SetBody(customizationName);
                    break;
            }
        }
    }

    public CustomizationType type;
    public string customizationName;

    public enum CustomizationType
    {
        Arms,
        Legs,
        Body,
        Head
    }

}
