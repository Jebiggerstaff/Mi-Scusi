using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToUnlockCosmetic : MonoBehaviour
{
    public string cosmeticName;
    public bool DestoryOnTouch = true;
    CosmeticUnlocker CosmeticUnlocker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CosmeticUnlocker == null)
            CosmeticUnlocker = FindObjectOfType<OverlayScene>().menu.GetComponent<CosmeticUnlocker>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            CosmeticUnlocker.UnlockOutfit(cosmeticName);
            if(DestoryOnTouch)
                Destroy(this.gameObject);
        }

    }
}
