using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class DesertTaskManager : MonoBehaviour
{

    public GameObject confetti;

    [Header("Tasks")]
    public GameObject[] Tasks;

    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskUpdatedText;
    public GameObject TaskList;
    public GameObject NextLevel;

    public GameObject StageGuard1;
    public GameObject StageGuard2;
    public GameObject StageGuard3;
    public GameObject StageGuard4;
    public GameObject GroundGuard1;
    public GameObject GroundGuard2;
    public GameObject GroundGuard3;
    public GameObject GroundGuard4;
    public List<GameObject> BurningManFire;
    public Text taskList;
    public NPC sailor;
    public NPC DJ;
    public GameObject fireworkStandGuy;
    public ExplodeBuilding fireworkStand;
    public GameObject music;
    [Space]
    public Animator ufo;
    public Animator sub;


    public CosmeticUnlocker CosmeticUnlocker;
    
    [HideInInspector] public int guardsRemoved = 0;
    [HideInInspector] public int AlienParts = 0;
    [HideInInspector] public int ShopThingsMessedWith = 0;
    [HideInInspector] public int speakersBroken = 0;
    [HideInInspector] public bool CanWinRace = false;
    [HideInInspector] public int shopFireworks = 0;
    [HideInInspector] public int GasCollected = 0;

    [HideInInspector]public bool[] TaskFinished;
    private bool menuOpen;


    string MainTask;

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = .2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);

        TaskFinished = new bool[Tasks.Length];
        for (int i = 0; i < TaskFinished.Length; i++)
        {
            TaskFinished[i] = false;
        }
        CanWinRace = false;

        menuOpen = true;
    }

    private MiScusiActions controls;
    private void Awake()
    {
        controls = new MiScusiActions();
        controls.Enable();
    }

    public void Update()
    {
        if (CosmeticUnlocker == null)
            CosmeticUnlocker = FindObjectOfType<OverlayScene>().menu.GetComponent<CosmeticUnlocker>();

        if (controls.UI.TaskMenu.triggered && menuOpen == false)
        {
            TaskCompleteText.SetActive(false);
            TaskList.SetActive(true);
            menuOpen = true;
            TaskUpdatedText.SetActive(false);
        }
        else if (controls.UI.TaskMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }

        MainTask = "";
        if(TaskFinished[0] == false)
        {
            MainTask = "Remove the guards protecting the Burning Man (" + guardsRemoved.ToString() + "/4)";
        }
        else
        {
            MainTask = "Burn down the Burning Man";
        }



        taskList.text = MainTask + "\n" +
        "Make a mess in the gift shop\n" +
        "Pause the music...forever\n" +
        "Send the submarine off to sea (" + GasCollected.ToString() + "/3)\n" +
        "\"Raise the roof\" at the fireworks stand\n" +
        "Win the foot race\n" +
        "Resolve the mystery in the cliffs\n";


        if(guardsRemoved >= 4)
        {
            TaskCompleted("GuardsRemoved");
        }
        
    }


    public void TaskCompleted(string Task)
    {

        switch (Task)
        {
            case "GuardsRemoved":
                if (TaskFinished[0] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    //Tasks[0].SetActive(true);
                    TaskFinished[0] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "BurnBurningMan":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());

                    foreach(var go in BurningManFire)
                    {
                        go.SetActive(true);
                    }

                    NextLevel.SetActive(true);

                    if (PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "").Contains("Burning") == false)
                    {
                        PlayerPrefs.SetString(PentagonTaskManager.CompletedExtras, PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "") + "Burning");
                    }
                }
                break;
            case "MessUpShop":
                if(TaskFinished[2] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[2].SetActive(true);
                    TaskFinished[2] = true;
                    StartCoroutine(confettistuff());


                    guardsRemoved++;
                    GroundGuard2.SetActive(true);
                    StageGuard2.SetActive(false);
                }
                break;
            case "Music":
                if (TaskFinished[3] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[3].SetActive(true);
                    TaskFinished[3] = true;
                    StartCoroutine(confettistuff());

                    Destroy(music);

                    guardsRemoved++;
                    GroundGuard3.SetActive(true);
                    StageGuard3.SetActive(false);

                    DJ.sentences[0] = "What happened to the music, man?";
                }
                break;
            case "Sailor":
                if (TaskFinished[4] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[4].SetActive(true);
                    TaskFinished[4] = true;
                    StartCoroutine(confettistuff());
                    sub.enabled = true;
                    guardsRemoved++;
                    GroundGuard1.SetActive(true);
                    StageGuard1.SetActive(false);
                    sailor.sentences = new string[1];
                    sailor.sentences[0] = "Well, guess I'm stuck here now...";
                }
                break;
            case "Fireworks":
                if (TaskFinished[5] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[5].SetActive(true);
                    TaskFinished[5] = true;
                    StartCoroutine(confettistuff());

                    fireworkStand.Explode();

                    guardsRemoved++;
                    GroundGuard4.SetActive(true);
                    StageGuard4.SetActive(false);


                    Destroy(fireworkStandGuy.gameObject);
                }
                break;
            case "Race":
                if (TaskFinished[6] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[6].SetActive(true);
                    TaskFinished[6] = true;
                    StartCoroutine(confettistuff());
                    CosmeticUnlocker.UnlockOutfit("DesertShirt");
                }
                break;
            case "Aliens":
                if (TaskFinished[7] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    Tasks[7].SetActive(true);
                    TaskFinished[7] = true;
                    ufo.enabled = true;
                    foreach(var cf in ufo.GetComponentsInChildren<ConfigurableJoint>())
                    {
                        Destroy(cf);
                    }
                    foreach (var rb in ufo.GetComponentsInChildren<Rigidbody>())
                    {
                        Destroy(rb);
                    }

                    foreach (var c in ufo.GetComponentsInChildren<Collider>())
                    {
                        Destroy(c);
                    }
                    StartCoroutine(confettistuff());
                    CosmeticUnlocker.UnlockOutfit("AlienHead");


                    if (PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "").Contains("Aliens") == false)
                    {
                        PlayerPrefs.SetString(PentagonTaskManager.CompletedExtras, PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "") + "Aliens");
                    }
                }
                break;
        }
    }

    IEnumerator confettistuff()
    {
        confetti.SetActive(true);
        yield return new WaitForSeconds(2);
        confetti.SetActive(false);
    }



    public void FireworkExploded()
    {
        Firework.numSpawned++;
        Invoke("reductionOfFirework", .5f);
    }
    void reductionOfFirework()
    {
        Firework.numSpawned--;
    }

}

