using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonCubePuzzle : MonoBehaviour
{
    public CruiseBtnHelper X;
    public CruiseBtnHelper Y;
    public CruiseBtnHelper Z;

    public GameObject cube;
    public GameObject sampleCube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (X.pressed)
            cube.transform.Rotate(30 * Time.deltaTime, 0, 0, Space.Self);
        if (Y.pressed)
            cube.transform.Rotate(0, 30 * Time.deltaTime, 0, Space.Self);
        if (Z.pressed)
            cube.transform.Rotate(0,0,30 * Time.deltaTime, Space.Self);

        sampleCube.transform.rotation = cube.transform.rotation;

    }
}
