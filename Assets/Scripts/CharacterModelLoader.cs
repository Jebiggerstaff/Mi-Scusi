using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(APRController))]
public class CharacterModelLoader : MonoBehaviour
{

    private void Awake()
    {
        //player = GetComponent<APRController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadAllModels();
        //StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAllModels()
    {
        if(PlayerPrefs.HasKey("Head"))
        {
            headModel = PlayerPrefs.GetString("Head");
        }
        else
        {
            headModel = "default";
        }
        if (PlayerPrefs.HasKey("Body"))
        {
            bodyModel = PlayerPrefs.GetString("Body");
        }
        else
        {
            bodyModel = "default";
        }
        if (PlayerPrefs.HasKey("Arms"))
        {
            armsModel = PlayerPrefs.GetString("Arms");
        }
        else
        {
            armsModel = "default";
        }
        if (PlayerPrefs.HasKey("Legs"))
        {
            legsModel = PlayerPrefs.GetString("Legs");
        }
        else
        {
            legsModel = "default";
        }

        loadModelsFromStrings();
    }

    private void loadModelsFromStrings()
    {
        Debug.Log(headModel + " " + bodyModel + " " + armsModel + " " + legsModel);


        //Resources.UnloadAsset(TargetCharacter.HeadCustomization);
        //Resources.UnloadAsset(TargetCharacter.BodyCustomization);
        //Resources.UnloadAsset(TargetCharacter.ArmsCustomization);
        //Resources.UnloadAsset(TargetCharacter.LegsCustomization);




        //setAModel(TargetCharacter.HeadCustomization, modelResourcesPath + headModel);
        //setAModel(TargetCharacter.BodyCustomization, modelResourcesPath + bodyModel;
        //setAModel(TargetCharacter.LeftArmLowerCustomization, modelResourcesPath + "Left" + armsModel + "Lower");
        //setAModel(TargetCharacter.LeftArmUpperCustomization, modelResourcesPath + "Left" + armsModel + "Upper");
        //setAModel(TargetCharacter.RightArmLowerCustomization, modelResourcesPath + "Right" + armsModel + "Lower");
        //setAModel(TargetCharacter.RightArmUpperCustomization, modelResourcesPath + "Right" + armsModel + "Upper");
        //setAModel(TargetCharacter.LeftLegLowerCustomization, modelResourcesPath + "Left" + legsModel + "Lower");
        //setAModel(TargetCharacter.LeftLegUpperCustomization, modelResourcesPath + "Left" + legsModel + "Upper");
        //setAModel(TargetCharacter.RightLegLowerCustomization, modelResourcesPath + "Right" + legsModel + "Lower");
        //setAModel(TargetCharacter.RightLegUpperCustomization, modelResourcesPath + "Right" + legsModel + "Upper");
        



    }

    private void setAModel(GameObject target, string path)
    {
        var getModel = Resources.Load<GameObject>(path);
        var getMesh = getModel.GetComponent<MeshFilter>().sharedMesh;
        var getMaterial = getModel.GetComponent<MeshRenderer>().sharedMaterial;

        target.GetComponent<MeshRenderer>().material = getMaterial;
        target.GetComponent<MeshFilter>().mesh = getMesh;
    }



    public IEnumerator Test()
    {
        

        bool yes = true;
        while(true)
        {
            if(yes)
            {
                var newGo = Resources.Load<GameObject>("Models/Cube");
                testMesh = newGo.GetComponent<MeshFilter>().sharedMesh;
                testMaterial = newGo.GetComponent<MeshRenderer>().sharedMaterial;
                yes = false;
            }
            else
            {
                var newGo = Resources.Load<GameObject>("Models/Sphere");
                testMesh = newGo.GetComponent<MeshFilter>().sharedMesh;
                testMaterial = newGo.GetComponent<MeshRenderer>().sharedMaterial;
                yes = true;
            }

            testPlayer.GetComponent<MeshFilter>().mesh = testMesh;
            testPlayer.GetComponent<MeshRenderer>().material = testMaterial;

            yield return new WaitForSeconds(1.0f);
        }

    }


    APRController player;

    public GameObject testPlayer;
    public Mesh testMesh;
    public Material testMaterial;

    private string headModel;
    private string bodyModel;
    private string legsModel;
    private string armsModel;

    public string modelResourcesPath;
}
