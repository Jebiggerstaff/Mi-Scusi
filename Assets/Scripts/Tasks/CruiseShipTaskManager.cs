using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CruiseShipTaskManager : MonoBehaviour
{

    public GameObject confetti;

    [Header("Tasks")]
    public GameObject[] Tasks = new GameObject[0];
    public Text MainText;
    public Text ChinaText;
    public Text EvacuateText;
    [Header("Gameobjects")]
    public GameObject Player;
    public GameObject TaskCompleteText;
    public GameObject TaskUpdatedText;
    public GameObject TaskList;

    
    public CosmeticUnlocker CosmeticUnlocker;
    private bool menuOpen;
    private bool[] TaskFinished = new bool[11];
    private string CurrentMainTask = "<b>Take Chef's Clothes as a disguise</b>";
    [HideInInspector] public int MenThrownInWater = 0;
    [HideInInspector] public int ChinaBroken = 0;
    [HideInInspector] public int BrigandsHit = 0;

    [Header("Audio Clips")]
    public AudioClip genericCompeltionClip;

    public void Start()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(0).GetComponent<Canvas>().planeDistance = 1.2f;
        Player = GameObject.Find("FinalPlayer");
        //Debug.Log("Found Player: " + Player);
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
        }
        else if (controls.UI.TaskMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }
        else if (controls.UI.PauseMenu.triggered && menuOpen == true)
        {
            TaskList.SetActive(false);
            menuOpen = false;
        }

        MainText.text = CurrentMainTask;
        EvacuateText.text = "Help Passengers \"Evacuate\" the Ship (" + (MenThrownInWater) + "/10)";
        ChinaText.text = "Claim insurace of the fine china (" + (ChinaBroken * 7) + "$/175$)";
        
    }

    public void TaskCompleted(string Task)
    {

        switch (Task)
        {
            case "TakeChefClothes":
                if (TaskFinished[0] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Tall Chef's Hat");
                    CosmeticUnlocker.UnlockOutfit("Chef Coat");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskUpdatedText.SetActive(true);
                    TaskCompleteText.SetActive(false);
                    TaskFinished[0] = true;
                    CurrentMainTask = CurrentMainTask = "<b>Hijack the ship</b>";
                    StartCoroutine(confettistuff());
                }
                break;
            case "HijackTheShip":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[0].SetActive(true);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "EvacuateShip":
                if (TaskFinished[2] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Tank Top");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[1].SetActive(true);
                    TaskFinished[2] = true;
                    StartCoroutine(confettistuff());
                    CosmeticUnlocker.UnlockOutfit("Speedo");
                }
                break;
            case "SavePrincess":
                if (TaskFinished[3] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Helmet");
                    CosmeticUnlocker.UnlockOutfit("Plate Armor");
                    CosmeticUnlocker.UnlockOutfit("Plate Legguards");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[2].SetActive(true);
                    TaskFinished[3] = true;
                    StartCoroutine(confettistuff());


                    if (PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "").Contains("Princess") == false)
                    {
                        PlayerPrefs.SetString(PentagonTaskManager.CompletedExtras, PlayerPrefs.GetString(PentagonTaskManager.CompletedExtras, "") + "Princess");
                    }
                }
                break;
            case "BreakChina":
                if (TaskFinished[4] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[3].SetActive(true);
                    TaskFinished[4] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "Waterslide":
                if (TaskFinished[5] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[4].SetActive(true);
                    TaskFinished[5] = true;
                    StartCoroutine(confettistuff());
                }
                break;
            case "Mutiny":
                if (TaskFinished[6] == false)
                {
                    CosmeticUnlocker.UnlockOutfit("Captain's Hat");
                    CosmeticUnlocker.UnlockOutfit("Captain's Jacket");
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    TaskCompleteText.SetActive(true);
                    TaskUpdatedText.SetActive(false);
                    Tasks[5].SetActive(true);
                    TaskFinished[6] = true;
                    StartCoroutine(confettistuff());
                }
                break;
        }
    }
    public void HitBrigand()
    {
        BrigandsHit++;
        if(BrigandsHit >= 6)
        {
            TaskCompleted("SavePrincess");
        }
    }

    IEnumerator confettistuff()
    {
        confetti.SetActive(true);
        yield return new WaitForSeconds(2);
        confetti.SetActive(false);
    }
}
