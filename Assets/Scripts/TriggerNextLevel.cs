using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Collider))]
public class TriggerNextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!canvasIsUp)
        {
            if (other.GetComponent<APRController>() != null || other.GetComponentInParent<APRController>() != null || other.GetComponentInChildren<APRController>() != null)
            {
                canvasIsUp = true;
                nextLevelCanvas.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                originalGameTIme = Time.timeScale;
                Time.timeScale = 0f;
            }
        }
        
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    public void loadMenu()
    {
        StartCoroutine(loadScene(mainMenuSceneName));
    }
    public void loadNextLevel()
    {
        StartCoroutine(loadScene(nextLevelSceneName));
    }
    

    private IEnumerator loadScene(string level)
    {
        Time.timeScale = originalGameTIme;
        //loadingScreen.SetActive(true);
        nextLevelCanvas.SetActive(false);
        AsyncOperation async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            fillBar.value = async.progress;
            yield return null;
        }

    }




    bool canvasIsUp = false;
    float originalGameTIme;

    [Header("Unity Things")]
    public GameObject nextLevelCanvas;
    public GameObject loadingScreen;
    public Slider fillBar;

    [Header("Scene Names")]
    public string mainMenuSceneName = "MenuTest";
    public string nextLevelSceneName;
}
