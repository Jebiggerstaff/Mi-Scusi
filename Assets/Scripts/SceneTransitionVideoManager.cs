using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionVideoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(videos != null)
        {
            getVideo();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            imSet = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!imSet)
        {
            if (videos != null)
            {
                getVideo();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                imSet = true;

            }
        }
    }

    void getVideo()
    {
        data = FindObjectOfType<CrossSceneData>();
        var vid = videos[data.videoIndex];

        if(data.videoIndex == 4)
        {
            if(data.prevIndex == 7)
            {
                vid = videos[20];    //OFFICE TO NY INSTEAD OF ITALY TO NY
            }
        }
        if(data.videoIndex == 10 )
        {
            if(data.prevIndex == 9)
            {
                vid = videos[19];       //TO RUSSIA
            }
            else
            {
                vid = videos[18];
            }
        }
        if(data.videoIndex == 2)
        {
            if (data.prevIndex == 5)
            {
                vid = videos[17];    //ITALY FROM NOIR
            }
        }


        foreach(var v in videos)
        {
            if(v != null)
            {
                if (v != vid)
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


    }

    public CrossSceneData data;
    public List<GameObject> videos;
    public LoadLevelButton btn;
    bool imSet = false;
}
