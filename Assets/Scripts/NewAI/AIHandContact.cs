using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIHandContact : MonoBehaviour
{


    float punchDelay = 0;

    public bool punching;

    private void Start()
    {
        
    }

    void Update()
    {
        punchDelay -= Time.deltaTime;
    }
    

    void OnCollisionEnter(Collision col)
    {
        if (punching)
        {

            if (col.gameObject.layer == LayerMask.NameToLayer("Player_1") && punchDelay <= 0)
            {

                Debug.Log("Punched Player");

                FindObjectOfType<APRController>().GotPunched();

                punchDelay = 1.0f;
            }

        }
    }

    public  void unPunch()
    {
        GetComponentInParent<HostileAI>().unPunch();
        Debug.Log("Unpunching");
    }

}
