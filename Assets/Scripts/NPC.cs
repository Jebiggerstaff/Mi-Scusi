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
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }
	
	void Update () {
          Vector3 Pos = Camera.main.WorldToScreenPoint(NPCCharacter.position);
     
        Pos.y += 175;
          ChatBackGround.position = Pos;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            this.gameObject.GetComponent<NPC>().enabled = true;
            FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
            if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
            {
                if (Name == "Former Gangster")
                    ItalyTaskManager.TaskCompleted("HearAboutMafia");

                this.gameObject.GetComponent<NPC>().enabled = true;
                dialogueSystem.Names = Name;
                dialogueSystem.dialogueLines = sentences;
                FindObjectOfType<DialogueSystem>().NPCName();
            }
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }
}

