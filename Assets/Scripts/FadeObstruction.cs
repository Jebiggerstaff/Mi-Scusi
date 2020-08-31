using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstruction : MonoBehaviour
{

    private void Awake()
    {
        objectsInWay = new List<GameObject>();
        objectsInWayCopy = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obstructionFadeUpdate();
        //SendRay(mainCam.transform.position, player.transform.position);
    }


    public void obstructionFadeUpdate()
    {
        objectsInWayCopy.Clear();
        foreach(var o in objectsInWay)
        {
            objectsInWayCopy.Add(o);
        }

        objectsInWay.Clear();

        float RayRadius = 0.5f;

        // Build up a list of objects that are in the way of all registered should show objects

        Vector3 diffVector = player.transform.position - cam.transform.position;
        Ray ray = new Ray(cam.transform.position, Vector3.Normalize(diffVector));
        RaycastHit[] hits = Physics.SphereCastAll(ray, RayRadius, diffVector.magnitude, 1 << GroundLayer | 1 << InvisibleGroundLayer);

        foreach (RaycastHit hit in hits)
        {
            // Get the collider
            Collider c = hit.collider;

            // skip any objects that should be visible
            if (c.gameObject == player || c.gameObject.GetComponent<Renderer>() == null)
                continue;

            if(c.GetComponent<PleaseFadeMe>() != null)
            {
                objectsInWay.Add(c.gameObject);
            }
        }

       // Debug.Log(objectsInWay);


        FadeObstructions();
        UnfadeNotObstruction();
    }


    public void FadeObstructions()
    {
        foreach(var o in objectsInWay)
        {
            o.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
    public void UnfadeNotObstruction()
    {
        foreach(var o in objectsInWayCopy)
        {
            if(!objectsInWay.Contains(o))
            {
                o.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
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
    List<GameObject> objectsInWay;
    List<GameObject> objectsInWayCopy;
    public GameObject player;
    public Camera cam;

    [Space]

    [Header("Layers")]
    public int GroundLayer = 14;
    public int InvisibleGroundLayer = 15;

    [Space]

    [Header("FadingInfo")]
    [Range(0.1f, 10f)]
    public float fadeDuration = 0.5f;
}
