﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public string splitTag;
    public string spawnedBeamTag;
    public int maxBounce;
    public int maxSplit;
    private float timer = 0;
    private LineRenderer mLineRenderer;
    public bool KillsAI;
    bool grabbed;
    [HideInInspector] public int grabCount;
    public AudioSource HoldNoise;
    public AudioClip HitNoise;
    public GameObject MoonDoor;
    public Material CompletedMaterial;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0)
        {
            grabbed = grabCount > 0;
            if (HoldNoise != null)
            {

                if (grabbed && !HoldNoise.isPlaying)
                    HoldNoise.Play();
                else if (!grabbed && HoldNoise.isPlaying)
                    HoldNoise.Stop();

            }
            if (gameObject.tag != spawnedBeamTag)
            {
                if (timer >= updateFrequency)
                {
                    timer = 0;
                    //Debug.Log("Redrawing laser");
                    foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag(spawnedBeamTag))
                        Destroy(laserSplit);

                    StartCoroutine(RedrawLaser());
                }
                timer += Time.deltaTime;
            }
        }
        
    }

    IEnumerator RedrawLaser()
    {
        //Debug.Log("Running");
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if (!KillsAI || (KillsAI && grabbed))
            while (loopActive)
            {
                //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
                if (Physics.SphereCast(lastLaserPosition, 0.5f, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == bounceTag) || (hit.transform.gameObject.tag == splitTag)))
                {
                    //Debug.Log("Bounce");
                    laserReflected++;
                    vertexCounter += 3;
                    mLineRenderer.SetVertexCount(vertexCounter);
                    mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                    mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                    mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                    mLineRenderer.SetWidth(.9f, .9f);
                    lastLaserPosition = hit.point;
                    Vector3 prevDirection = laserDirection;
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);

                    if (hit.transform.gameObject.tag == splitTag)
                    {
                        //Debug.Log("Split");
                        if (laserSplit >= maxSplit)
                        {
                            Debug.Log("Max split reached.");
                        }
                        else
                        {
                            //Debug.Log("Splitting...");
                            laserSplit++;
                            Object go = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
                            go.name = spawnedBeamTag;
                            ((GameObject)go).tag = spawnedBeamTag;
                        }
                    }
                }
                else
                {
                    //Debug.Log("No Bounce");
                    laserReflected++;
                    vertexCounter++;
                    mLineRenderer.SetVertexCount(vertexCounter);
                    Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                    //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);



                    if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))
                    {
                        mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                        if (KillsAI && hit.collider.gameObject.GetComponent<NewAIMan>() != null)
                        {
                            GetComponent<Raygun>().SpawnEffects(hit.collider.gameObject.transform.position);
                            RandomAudioMaker.makeAudio(HitNoise, 0.5f);
                            if (hit.collider.gameObject.name == "MafiaBoss")
                            {
                                FindObjectOfType<PentagonTaskManager>().TaskCompleted("Mafia");
                            }
                            if (hit.collider.gameObject.name == "SSS")
                            {
                                FindObjectOfType<PentagonTaskManager>().TaskCompleted("SSS");
                            }
                            if (hit.collider.gameObject.name == "Princess")
                            {
                                FindObjectOfType<PentagonTaskManager>().TaskCompleted("Princess");
                            }
                            Destroy(hit.collider.gameObject);
                        }
                        if(hit.collider.tag == "LaserButton" && !KillsAI)
                        {
                            mLineRenderer.material = CompletedMaterial;

                            if (FindObjectOfType<PentagonTaskCollider>() != null)
                                FindObjectOfType<PentagonTaskManager>().TaskCompleted("Door");
                            else
                                Destroy(MoonDoor);
                        }
                    }
                    else
                    {
                        mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));
                    }


                    loopActive = false;
                }
                if (laserReflected > maxBounce)
                    loopActive = false;
            }

        yield return new WaitForEndOfFrame();
    }
}
