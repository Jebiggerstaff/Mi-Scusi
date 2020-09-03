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

    private void TurnOffAll()
    {
        head1.SetActive(false);
        head2.SetActive(false);
        head3.SetActive(false);
        head4.SetActive(false);
        head5.SetActive(false);
        head6.SetActive(false);

        left_leg1.SetActive(false);
        left_leg2.SetActive(false);
        left_leg3.SetActive(false);
        left_leg4.SetActive(false);
        left_leg5.SetActive(false);
        left_leg6.SetActive(false);
        
        right_leg1.SetActive(false);
        right_leg2.SetActive(false);
        right_leg3.SetActive(false);
        right_leg4.SetActive(false);
        right_leg5.SetActive(false);
        right_leg6.SetActive(false);
        
        left_arm1.SetActive(false);
        left_arm2.SetActive(false);
        left_arm3.SetActive(false);
        left_arm4.SetActive(false);
        left_arm5.SetActive(false);
        left_arm6.SetActive(false);
        
        right_arm1.SetActive(false);
        right_arm2.SetActive(false);
        right_arm3.SetActive(false);
        right_arm4.SetActive(false);
        right_arm5.SetActive(false);
        right_arm6.SetActive(false);
    }


    private void loadModelsFromStrings()
    {
        Debug.Log(headModel + " " + bodyModel + " " + armsModel + " " + legsModel);

        TurnOffAll();

        switch(headModel)
        {
            case "1":
                head1.SetActive(true);
                break;
            case "2":
                head2.SetActive(true);
                break;
            case "3":
                head3.SetActive(true);
                break;
            case "4":
                head4.SetActive(true);
                break;
            case "5":
                head5.SetActive(true);
                break;
            case "6":
                head6.SetActive(true);
                break;
           
        }

        switch (bodyModel)
        {
            case "1":
                body1.SetActive(true);
                break;
            case "2":
                body2.SetActive(true);
                break;
            case "3":
                body3.SetActive(true);
                break;
            case "4":
                body4.SetActive(true);
                break;
            case "5":
                body5.SetActive(true);
                break;
            case "6":
                body6.SetActive(true);
                break;

        }

        switch (legsModel)
        {
            case "1":
                left_leg1.SetActive(true);
                right_leg1.SetActive(true);
                break;
            case "2":
                left_leg2.SetActive(true);
                right_leg2.SetActive(true);
                break;
            case "3":
                left_leg3.SetActive(true);
                right_leg3.SetActive(true);
                break;
            case "4":
                left_leg4.SetActive(true);
                right_leg4.SetActive(true);
                break;
            case "5":
                left_leg5.SetActive(true);
                right_leg5.SetActive(true);
                break;
            case "6":
                left_leg6.SetActive(true);
                right_leg6.SetActive(true);
                break;

        }

        switch (armsModel)
        {
            case "1":
                left_arm1.SetActive(true);
                right_arm1.SetActive(true);
                break;
            case "2":
                left_arm2.SetActive(true);
                right_arm2.SetActive(true);
                break;
            case "3":
                left_arm3.SetActive(true);
                right_arm3.SetActive(true);
                break;
            case "4":
                left_arm4.SetActive(true);
                right_arm4.SetActive(true);
                break;
            case "5":
                left_arm5.SetActive(true);
                right_arm5.SetActive(true);
                break;
            case "6":
                left_arm6.SetActive(true);
                right_arm6.SetActive(true);
                break;

        }











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


    public GameObject head1;
    public GameObject head2;
    public GameObject head3;
    public GameObject head4;
    public GameObject head5;
    public GameObject head6;

    public GameObject body1;
    public GameObject body2;
    public GameObject body3;
    public GameObject body4;
    public GameObject body5;
    public GameObject body6;

    public GameObject left_arm1;
    public GameObject left_arm2;
    public GameObject left_arm3;
    public GameObject left_arm4;
    public GameObject left_arm5;
    public GameObject left_arm6;

    public GameObject right_arm1;
    public GameObject right_arm2;
    public GameObject right_arm3;
    public GameObject right_arm4;
    public GameObject right_arm5;
    public GameObject right_arm6;

    public GameObject right_leg1;
    public GameObject right_leg2;
    public GameObject right_leg3;
    public GameObject right_leg4;
    public GameObject right_leg5;
    public GameObject right_leg6;

    public GameObject left_leg1;
    public GameObject left_leg2;
    public GameObject left_leg3;
    public GameObject left_leg4;
    public GameObject left_leg5;
    public GameObject left_leg6;

    private string headModel;
    private string bodyModel;
    private string legsModel;
    private string armsModel;
    
}
