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

    private DialogueSystem dialogueSystem;

    public string Name;

    [TextArea(5, 10)]
    public string[] sentences;

    void Start () {
        if(SceneManager.GetActiveScene().name == "Italy")
            ItalyTaskManager = GameObject.Find("TaskUI").GetComponent<ItalyTaskManager>();
        dialogueSystem = DialogueSystem.dialogueSystem;
    }
	
	void Update () {
          Vector3 Pos = Camera.main.WorldToScreenPoint(NPCCharacter.position);
     
        Pos.y += 175;
          ChatBackGround.position = Pos;

        if(dialogueSystem == null)
        {
            dialogueSystem = DialogueSystem.dialogueSystem;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            this.gameObject.GetComponent<NPC>().enabled = true;
            DialogueSystem.dialogueSystem.EnterRangeOfNPC();
            if ((other.gameObject.tag == "Player") && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
            {
                if (Name == "Former Gangster")
                    ItalyTaskManager.TaskCompleted("HearAboutMafia");

                this.gameObject.GetComponent<NPC>().enabled = true;
                dialogueSystem.Names = Name;
                dialogueSystem.dialogueLines = sentences;
                DialogueSystem.dialogueSystem.NPCName();
            }
        }
    }

    public void OnTriggerExit()
    {
        DialogueSystem.dialogueSystem.OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }
}

