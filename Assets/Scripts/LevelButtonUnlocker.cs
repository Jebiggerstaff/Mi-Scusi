using SpeedTutorMainMenuSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUnlocker : MonoBehaviour
{
    public string LevelName;
    Button btn;
    Image img;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        
        if(PlayerPrefs.GetString(MenuController.LevelUnlockedPref, "").Contains(MenuController.LevelBuffer + LevelName + MenuController.LevelBuffer))
        {
            btn.interactable = true;
            img.color = Color.white;
            
        }
        else
        {
            btn.interactable = false;
            img.color = Color.white;
            img.color = new Color(img.color.r, img.color.g, img.color.b, .75f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
