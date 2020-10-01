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
            switch (type)
            {
                case CustomizationType.Hat:
                    mc.SetHat(customizationIndex);
                    break;
                case CustomizationType.Shirt:
                    mc.SetShirt(customizationIndex);
                    break;
                case CustomizationType.Coat:
                    mc.SetCoat(customizationIndex);
                    break;
                case CustomizationType.Pants:
                    mc.SetPants(customizationIndex);
                    break;
                case CustomizationType.Backpack:
                    mc.SetBackpack(customizationIndex);
                    break;
                case CustomizationType.Accessory:
                    mc.SetAccessory(customizationIndex);
                    break;
            }
            
        }
    }

    public CustomizationType type;
    public int customizationIndex;

    public enum CustomizationType
    {
        Hat,
        Shirt,
        Coat,
        Pants,
        Backpack,
        Accessory
    }

}
