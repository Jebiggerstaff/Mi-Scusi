using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NPC : MonoBehaviour {

    ItalyTaskManager ItalyTaskManager = new ItalyTaskManager();

    public Transform ChatBackGround;
    public Transform NPCCharacter;
    bool inRange;

    public bool DEBUG_printDistance = false;

    private DialogueSystem dialogueSystem;

    public string Name;

    float speakRange = 8f;
    Transform player;

    [TextArea(5, 10)]
    public string[] sentences;

    void Start () {
        if(SceneManager.GetActiveScene().name == "Italy")
            ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();
        dialogueSystem = DialogueSystem.dialogueSystem;
        player = FindObjectOfType<APRController>().Root.transform;

        speakRange = 8f;
    }
	
	void Update () {
          Vector3 Pos = Camera.main.WorldToScreenPoint(NPCCharacter.position);
     
        Pos.y += 175;
          ChatBackGround.position = Pos;

        if(dialogueSystem == null)
        {
            dialogueSystem = DialogueSystem.dialogueSystem;
        }

        RangeCheck();
    }



    void RangeCheck()
    {
        if(DEBUG_printDistance)
        {

        }
        if (Vector3.Distance(transform.position, player.position) <= speakRange)
        {
            InRange();
            
        }
        else
        {
            outOfRange();
        }
    }

    void InRange()
    {
        DialogueSystem.dialogueSystem.EnterRangeOfNPC();
        if ((DialogueSystem.dialogueSystem.controls.Player.Interact.triggered))
        {
            if (Name == "Former Gangster")
                ItalyTaskManager.TaskCompleted("HearAboutMafia");

            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = sentences;
            DialogueSystem.dialogueSystem.NPCName();
        }
        inRange = true;
    }
    void outOfRange()
    {
        if(inRange)
        {
            inRange = false;

            DialogueSystem.dialogueSystem.OutOfRange();

        }
    }
    
}

