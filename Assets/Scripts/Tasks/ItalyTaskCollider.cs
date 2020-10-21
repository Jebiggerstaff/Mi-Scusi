using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    ItalyTaskManager ItalyTaskManager = new ItalyTaskManager();

    public void Start()
    {
        ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();
    }

    public void OnTriggerEnter(Collider other)
    {

    }
}
