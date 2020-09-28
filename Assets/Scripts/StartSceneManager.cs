using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        doStuff();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void doStuff()
    {
        var os = FindObjectOfType<OverlayScene>();
        if (os == null)
        {
            os = Instantiate(overlayStuff, new Vector3(0,0,0), Quaternion.Euler(0,0,0)).GetComponent<OverlayScene>();
        }

        if (isMenuScene)
        {
            Time.timeScale = 0.0001f;
            os.menu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            os.menu.SetActive(false);
        }

    }

    public GameObject overlayStuff;
    public bool isMenuScene = false;
    public Vector3 playerSpawnLocation;
}