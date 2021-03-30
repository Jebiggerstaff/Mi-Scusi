using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRioGuard : MonoBehaviour
{
    RioTaskManager m;

    // Start is called before the first frame update
    void Start()
    {
        m = FindObjectOfType<RioTaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m.TaskFinished[1])
        {
            Destroy(gameObject);
        }
    }
}
