using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneTransitionVideoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        getVideo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getVideo()
    {
        data = FindObjectOfType<CrossSceneData>();
        var vid = videos[data.videoIndex];
        foreach(var v in videos)
        {
            if(v != vid)
            {
                v.SetActive(false);
            }
            else
            {
                currentVideo = v.GetComponent<VideoPlayer>();
                if(currentVideo.isPlaying == false)
                {
                    currentVideo.Play();
                }
            }
        }

        InvokeRepeating("SwapCheck", 1.5f, 0.25f);

    }
    void SwapCheck()
    {
        if (currentVideo.isPlaying == false)
        {
            StartCoroutine(SwitchScene());
        }
    }
    IEnumerator SwitchScene()
    {
        string level = data.sceneName;
        Destroy(data.gameObject);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(level);
    }

    public CrossSceneData data;
    public List<GameObject> videos;
    public VideoPlayer currentVideo;
}
