using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RussiaTaskManager : MonoBehaviour
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
    public Text taskList;
    [Space]
    public Rigidbody Bed;
    public Rigidbody Chair;
    public Rigidbody Table;
    public NPC Brunettedilocks;
    int correctSizeCount;
    public HostileAI Monster;
    public GameObject Explosion;
    [Space]
    public AudioClip[] explosions;
    [Space]
    public CosmeticUnlocker CosmeticUnlocker;

    [HideInInspector] public bool[] TaskFinished;
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
            MainTask = "Break into the factory";
        }
        else
        {
            MainTask = "Destroy the factory";
        }

        taskList.text = MainTask + "\n" +
            "Slay the radioactive monster\n" + 
            "Acquire a larger than life portrait of yourself\n" + 
            "Help make everything just right";




        correctSizeCount = 0;


        #region Brunttedilocks
        if(Bed.mass >= 64f)
        {
            correctSizeCount++;
            Brunettedilocks.sentences[1] = "My bed is just right!";
        }
        else
        {
            Brunettedilocks.sentences[1] = "My bed is too small!";
        }

        if(Chair.mass == 8f)
        {
            correctSizeCount++;
            Brunettedilocks.sentences[2] = "My chair is just right!";
        }
        else if(Chair.mass < 8)
        {

            Brunettedilocks.sentences[2] = "My chair is too small!";
        }
        else
        {

            Brunettedilocks.sentences[2] = "My chair is too big!";
        }

        if (Table.mass <= 1)
        {
            correctSizeCount++;
            Brunettedilocks.sentences[3] = "My table is just right!";
        }
        else
        {
            Brunettedilocks.sentences[3] = "My table is too big!";
        }


        if (correctSizeCount == 0)
            Brunettedilocks.sentences[0] = "Oh, none of this is just right!";
        else if (correctSizeCount < 3)
            Brunettedilocks.sentences[0] = "Well, at least some of this is just right!";
        else
        {
            Brunettedilocks.sentences[0] = "Everything is just right!";
            TaskCompleted("Brunettedilocks");
        }
        #endregion
        #region Monster
        if(Monster.gameObject.activeSelf && Monster.stunCount > 0)
        {
            TaskCompleted("Monster");
        }
        #endregion


    }

    IEnumerator explosionsSounds()
    {
        while(true)
        {
            RandomAudioMaker.makeAudio(explosions[Random.Range(0, explosions.Length)], 0.5f);
            yield return new WaitForSeconds(Random.Range(0.25f, 1f));
        }
    }


    public void TaskCompleted(string Task)
    {
        switch (Task)
        {
            case "BreakIn":
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
            case "DestroyFactory":
                if (TaskFinished[1] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[1].SetActive(true);
                    TaskFinished[1] = true;
                    StartCoroutine(confettistuff());

                    Explosion.SetActive(true);

                    StartCoroutine(explosionsSounds());

                    NextLevel.SetActive(true);
                }
                break;
            case "Monster":
                if (TaskFinished[2] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[2].SetActive(true);
                    TaskFinished[2] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "Portrait":
                if (TaskFinished[3] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);
                    CosmeticUnlocker.UnlockOutfit("Horse Head");
                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[3].SetActive(true);
                    TaskFinished[3] = true;
                    StartCoroutine(confettistuff());

                }
                break;
            case "Brunettedilocks":
                if (TaskFinished[4] == false)
                {
                    RandomAudioMaker.makeAudio(genericCompeltionClip);

                    TaskUpdatedText.SetActive(false);
                    TaskCompleteText.SetActive(true);
                    Tasks[4].SetActive(true);
                    TaskFinished[4] = true;
                    StartCoroutine(confettistuff());

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

}
