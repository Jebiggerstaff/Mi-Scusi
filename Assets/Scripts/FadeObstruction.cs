using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SendRay(mainCam.transform.position, player.transform.position);
    }


    public void SendRay(Vector3 origin, Vector3 target)
    {
        int layerMask = 1 << GroundLayer;
        Ray ray = new Ray(origin, target - origin);
        RaycastHit hit;
        Debug.DrawRay(origin, target - origin, Color.red, Time.deltaTime * 1.1f);
        if(Physics.Raycast(ray, out hit, (target - origin).magnitude, layerMask))
        {
            MakeInvisible(hit.collider.gameObject);
            SendRay(hit.point, target);
        }
    }
    public void MakeInvisible(GameObject go)
    {
        go.layer = InvisibleGroundLayer;
        StartCoroutine(makeVisible(go));
    }

    IEnumerator makeVisible(GameObject go)
    {
        yield return new WaitForSeconds(fadeDuration);
        go.layer = GroundLayer;
    }

    [Header("PlayerInfo")]
    public Camera mainCam;
    public GameObject player;

    [Space]

    [Header("Layers")]
    public int GroundLayer = 14;
    public int InvisibleGroundLayer = 15;

    [Space]

    [Header("FadingInfo")]
    [Range(0.1f, 10f)]
    public float fadeDuration = 0.5f;
}
