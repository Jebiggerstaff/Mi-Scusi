using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        StartCoroutine(InitialSceneFade());
    }

    IEnumerator InitialSceneFade()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        FadeScene(true);
    }

    public void FadeScene(bool fadeIn)
    {
        sceneFader.SetBool("Stuck", false);
        sceneFader.SetBool("FadingIn", fadeIn);

    }


    public void swapScenes(string levelname)
    {

        bool goToTransition = true;

        if(levelname == "Menu")
        {
            SceneManager.LoadScene(levelname);
            goToTransition = false;
        }


        var dataO = Instantiate(dataPrefab);
        DontDestroyOnLoad(dataO);
        var data = dataO.GetComponent<CrossSceneData>();
        data.sceneName = levelname;

        
        switch (levelname)
        {
            case "Tutorial":
                goToTransition = true;
                //SceneManager.LoadScene(levelname);
                data.videoIndex = 7;
                break;
            case "Italy":
                data.videoIndex = 0;
                break;
            case "NewYork":
                data.videoIndex = 1;
                break;
            case "Noir":
                data.videoIndex = 2;
                break;
            case "Desert":
                data.videoIndex = 3;
                break;
            case "Office 1":
                data.videoIndex = 4;
                break;
            case "Ship":
                data.videoIndex = 5;
                break;
            case "Rio":
                data.videoIndex = 6;
                break;
            default:
                data.videoIndex = 0;
                break;
        }
        if(goToTransition)
        {

            SceneManager.LoadScene(TransitionScene);

        }
    }



    public GameObject overlayStuff;
    public bool isMenuScene = false;
    public Vector3 playerSpawnLocation;

    public Animator sceneFader;


    public GameObject dataPrefab;
    public string TransitionScene = "SceneTransitions";
}