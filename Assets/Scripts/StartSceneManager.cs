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
            os = Instantiate(overlayStuff).GetComponent<OverlayScene>();
        }

        if (isMenuScene)
        {
            os.player.SetActive(false);
            os.menu.SetActive(true);
        }
        else
        {
            os.player.SetActive(true);
            os.menu.SetActive(false);
            os.player.transform.position = playerSpawnLocation;
        }
    }

    public GameObject overlayStuff;
    public bool isMenuScene = false;
    public Vector3 playerSpawnLocation;
}