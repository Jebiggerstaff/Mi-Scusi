using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticOnKnockout : MonoBehaviour
{
    public string cosmeticName;
    CosmeticUnlocker CosmeticUnlocker;
    NewAIMan ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<NewAIMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CosmeticUnlocker == null)
            CosmeticUnlocker = FindObjectOfType<OverlayScene>().menu.GetComponent<CosmeticUnlocker>();
        if(ai.stunCount > 0)
        {
            CosmeticUnlocker.UnlockOutfit(cosmeticName);
        }
    }

    
}





