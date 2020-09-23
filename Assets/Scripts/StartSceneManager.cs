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
            os.playermain.SetActive(false);
            os.menu.SetActive(true);
            Debug.Log(Physics.gravity);
        }
        else
        {
            Time.timeScale = 1;
            os.menu.SetActive(false);
            os.playermain.SetActive(true);
            os.player.GetComponentInParent<APRController>().cam = Camera.main;
        }

        os.playermain.transform.position = playerSpawnLocation;
        os.player.transform.localPosition = new Vector3(0, 0, 0);
        os.playerother.transform.localPosition = new Vector3(0, 0, 0);
    }

    public GameObject overlayStuff;
    public bool isMenuScene = false;
    public Vector3 playerSpawnLocation;
}