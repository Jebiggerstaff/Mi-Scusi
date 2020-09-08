using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;
    public Vector3 CamPos;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    RaycastHit hit;

    private void Awake()
    {
        CamPos = Offset;
    }

    private void Update()
    {
        YeetThatCamera();
        /*
        if (Physics.Linecast(transform.position, Player.position,out hit))
        {
            if (hit.transform.tag == "Buildings")
            {
                Camera.main.transform.position += Camera.main.transform.forward * 1f; ;
            }
            else
            {
                
            }
        }
        */
    }

    void LateUpdate()
    {
        /*
        if (Player != null)
            transform.position = Vector3.SmoothDamp(transform.position, Player.position+Offset, ref velocity, smoothTime);
        */
    }

    void YeetThatCamera()
    {
        RaycastHit []hits;
        Ray ray = new Ray(Player.transform.position, Offset);
        hits = Physics.SphereCastAll(ray, 0.25f, Offset.magnitude);
        hits = removeNonBuildings(hits);
        sortHitsByDistance(hits);

        if(hits.Length > 0)
        {
            transform.position = Vector3.Lerp(transform.position, hits[0].point, Time.deltaTime * 4);
        }
        else
        {
            /*
            if(Vector3.Distance(transform.position, Player.position + Offset) < 2)
            {

                transform.position = Vector3.MoveTowards(transform.position, Player.position + Offset, 10 * Time.deltaTime);
            }
            else
            {

                transform.position = Vector3.MoveTowards(transform.position, Player.position + Offset, 50 * Time.deltaTime);
            }
            */

            transform.position = Vector3.Lerp(transform.position, Player.position + Offset, Time.deltaTime * 4);
        }
    }

    RaycastHit[] removeNonBuildings(RaycastHit[] hits)
    {
        var hList = new List<RaycastHit>(hits);
        for(int i = 0; i < hList.Count; i++)
        {
            if (hList[i].transform.tag != "Buildings")
            {
                hList.RemoveAt(i);
                i--;
            }
        }
        return hList.ToArray();
    }
    void sortHitsByDistance(RaycastHit []hits)
    {
        float dist1, dist2;
        for(int i = 0; i < hits.Length; i++)
        {
            for(int j = 0;  j < hits.Length - i - 1; j++)
            {
                dist1 = Vector3.Distance(hits[j].point, Player.position);
                dist2 = Vector3.Distance(hits[j + 1].point, Player.position);
                if(dist1 > dist2)
                {
                    RaycastHit temp = hits[j];
                    hits[j] = hits[j + 1];
                    hits[j + 1] = temp;
                }
            }
        }
    }

}
