using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        makeHPs();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        updateHP();
        updateVisuals();
    }


    public void makeHPs()
    {
        float width = hpArea.rect.width;
        float height = hpArea.rect.height * 0.9f;
        baseAreaVisibility = hpArea.GetComponent<Image>().color.a;
        hpbars = new List<GameObject>();
        for(int i = 0; i < player.maxHP; i++)
        {
            hpbars.Add(Instantiate(hpPrefab, hpArea.transform));
            var rt = hpbars[i].GetComponent<RectTransform>();
            
            int offsetNumber = -(player.maxHP/2) + i;

            rt.localPosition = new Vector3(offsetNumber * ((width/player.maxHP)), 0, 0);
            rt.sizeDelta = new Vector2(((width / player.maxHP) * 0.9f), height);
            visualTimer = 5;
        }

    }

    void updateHP()
    {
        for(int i = 0; i < player.maxHP; i++)
        {
            if(i < player.maxHP - player.currentHP)
            {

                if (hpbars[i].activeSelf == true)
                {
                    visualTimer = 2;
                    hpbars[i].SetActive(false);
                }
            }
            else
            {
                if (hpbars[i].activeSelf == false)
                {
                    visualTimer = 2;
                    hpbars[i].SetActive(true);
                }
            }
            
        }
    }
    void updateVisuals()
    {
        visualTimer -= Time.deltaTime;
        foreach(var h in hpbars)
        {
            h.GetComponent<Image>().color = new Color(h.GetComponent<Image>().color.r, h.GetComponent<Image>().color.g, h.GetComponent<Image>().color.b, Mathf.Clamp(visualTimer, 0, 1));
        }
        hpArea.GetComponent<Image>().color = new Color(0, 0, 0, baseAreaVisibility * Mathf.Clamp(visualTimer, 0, 1));
    }


    public GameObject hpPrefab;

    public APRController player;

    public RectTransform hpArea;
    List<GameObject> hpbars;

    float visualTimer;
    float baseAreaVisibility;
}
