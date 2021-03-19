using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToUnlockCosmetic : MonoBehaviour
{
    string cosmeticName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
        {
            FindObjectOfType<CosmeticUnlocker>().UnlockOutfit(cosmeticName);
            Destroy(this.gameObject);
        }

    }
}
