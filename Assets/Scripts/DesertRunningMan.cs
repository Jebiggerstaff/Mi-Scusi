﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertRunningMan : MonoBehaviour
{
    public Vector3 target = new Vector3(41.2000008f, 19.2299995f, -85f);
    public Vector3 origTarget = new Vector3(164.649994f, 19.2299995f, 125.809998f);

    public NewAIMan aiMan;
    public NPC speech;
    public float targetSpeed = 3f;

    bool readyToRace = true;
    bool racing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1)
        {
            aiMan.agent.speed = targetSpeed;
            if (!racing && !readyToRace)
                aiMan.agent.speed /= 2;
        }
        else
        {
            aiMan.agent.speed = targetSpeed / 2;
            if (!racing && !readyToRace)
                aiMan.agent.speed /= 2;
        }

        if(racing)
        {
            if(Vector3.Distance(transform.position, target) <= 4)
            {
                RestartRace();
            }
        }
        if(!racing && !readyToRace)
        {
            if (Vector3.Distance(transform.position, origTarget) <= 4)
            {
                BackToStart();
            }
        }
    }

    public void StartRace()
    {
        if(readyToRace)
        {

            SetTarget(target);
            speech.sentences = new string[1];
            speech.sentences[0] = "You can't catch me, amigo!";
            readyToRace = false;
            racing = true;
            FindObjectOfType<DesertTaskManager>().CanWinRace = true;

        }
    }
    public void RestartRace()
    {
        SetTarget(origTarget);
        speech.sentences[0] = "Good race, amigo!  Let's do it again sometime!";
        racing = false;
        FindObjectOfType<DesertTaskManager>().CanWinRace = false;

    }

    public void BackToStart()
    {
        if(!readyToRace)
        {

            readyToRace = true;
            speech.sentences = new string[3];
            speech.sentences[0] = "I'm the fastest corredor in this festival!  (cont)";
            speech.sentences[1] = "If you think you can keep up with my velocidad, then let's race! First one to the other side of these beams? (cont)";
            speech.sentences[2] = "As soon as this conversation ends, dalé güey!  And no funny business, I'll shove you!";

        }
    }

    void SetTarget(Vector3 t)
    {
        aiMan.destinations = new List<Vector3>();
        aiMan.destinations.Add(t);
        aiMan.forceNewDest();
    }
}
