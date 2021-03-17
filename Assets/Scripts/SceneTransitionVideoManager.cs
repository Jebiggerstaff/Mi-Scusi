﻿using System.Collections;
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

        if(data.videoIndex == 4)
        {
            if(data.prevIndex != 2)
            {
                vid = videos[20];    //OFFICE TO NY INSTEAD OF ITALY TO NY
            }
        }
        if(data.videoIndex == 10)
        {
            if(data.prevIndex == 9)
            {
                vid = videos[19];
            }
            else
            {
                vid = videos[18];
            }
        }

        foreach(var v in videos)
        {
            if(v != vid)
            {
                v.SetActive(false);
            }
            else
            {
                v.SetActive(true);

                btn.pics = v.GetComponent<LevelImageHolder>().pictures;
                btn.index = data.videoIndex;
                btn.picsSet = true;
            }
        }


    }

    public CrossSceneData data;
    public List<GameObject> videos;
    public LoadLevelButton btn;
}
