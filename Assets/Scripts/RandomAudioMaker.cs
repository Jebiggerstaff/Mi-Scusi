using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public static void makeAudio(AudioClip clip)
    {

        makeAudio(clip, 1);

    }

    public static void makeAudio(AudioClip clip, float volume)
    {
        var go = Instantiate(new GameObject());
        go.AddComponent<AudioSource>();
        go.transform.position = FindObjectOfType<APRController>().transform.position;
        go.GetComponent<AudioSource>().clip = clip;
        go.GetComponent<AudioSource>().loop = false;

        go.GetComponent<AudioSource>().volume = volume;

        go.GetComponent<AudioSource>().Play();
        
        Destroy(go, clip.length * 1.5f);
    }
}
