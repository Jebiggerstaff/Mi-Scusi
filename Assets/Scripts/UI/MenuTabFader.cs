using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTabFader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dim()
    {
        img.color = new Color(.5f, .5f, .5f);
    }
    public void Brighten()
    {
        img.color = new Color(1, 1, 1);
    }

    Image img;
}
