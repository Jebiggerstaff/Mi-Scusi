using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionVideoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        getVideo();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
                v.SetActive(true);
            }
        }

        //InvokeRepeating("SwapCheck", 1.5f, 0.25f);

    }

    public CrossSceneData data;
    public List<GameObject> videos;
}
