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
            cube.transform.rotation = Quaternion.Euler(cube.transform.rotation.eulerAngles + new Vector3(30 * Time.deltaTime, 0, 0));
        if (Y.pressed)
            cube.transform.rotation = Quaternion.Euler(cube.transform.rotation.eulerAngles + new Vector3(0, 30 * Time.deltaTime, 0));
        if (Z.pressed)
            cube.transform.rotation = Quaternion.Euler(cube.transform.rotation.eulerAngles + new Vector3(0, 0, 30 * Time.deltaTime));

        sampleCube.transform.rotation = cube.transform.rotation;

    }
}
