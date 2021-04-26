using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueSystem: MonoBehaviour {

    [HideInInspector]
    public MiScusiActions controls;

    public Text nameText;
    public Text dialogueText;

    public static DialogueSystem dialogueSystem;

    public GameObject dialogueGUI;
    public Transform dialogueBoxGUI;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    public string Names;

    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;

    public AudioClip audioClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dialogueText.text = "";
        dialogueSystem = this;

        controls = new MiScusiActions();
        controls.Enable();


        dialogueBoxGUI.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public void EnterRangeOfNPC()
    {
        outOfRange = false;
        dialogueGUI.SetActive(true);
        if (dialogueActive == true)
        {
            dialogueGUI.SetActive(false);
        }
    }

    public void NPCName()
    {
        outOfRange = false;
        dialogueBoxGUI.gameObject.SetActive(true);
        nameText.text = Names;
        if (controls.Player.Interact.triggered)
        {
            if (!dialogueActive)
            {
                dialogueActive = true;
                StartCoroutine(StartDialogue());
            }
        }
        StartDialogue();
    }

    private IEnumerator StartDialogue()
    {
        if (outOfRange == false)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;

            while (currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if (currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;


                        

                    }
                    
                }
                yield return 0;
            }

            while (true)
            {
                if (controls.Player.Interact.triggered && dialogueEnded == false)
                {
                    
                    if (Names == "Speedy Gonzo")
                    {
                        FindObjectOfType<DesertRunningMan>().StartRace();
                    }
                    if(Names == "John Elton")
                    {
                        FindObjectOfType<MoonTaskManager>().CosmeticUnlocker.UnlockOutfit("Space Helmet");
                        FindObjectOfType<MoonTaskManager>().CosmeticUnlocker.UnlockOutfit("Space Pants");
                        FindObjectOfType<MoonTaskManager>().CosmeticUnlocker.UnlockOutfit("Space Pack");
                        FindObjectOfType<MoonTaskManager>().CosmeticUnlocker.UnlockOutfit("Space Suit");


                    }



                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        bool skipDelay = false;
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";

            while (currentCharacterIndex < stringLength)
            {

                


                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;


                if (currentCharacterIndex < stringLength)
                {
                    if (controls.Player.Interact.triggered)
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);

                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                }
                else
                {
                    dialogueEnded = false;
                    break;
                }
                if (controls.Player.Interact.triggered)
                {
                    dialogueText.text = stringToDisplay;
                    currentCharacterIndex = stringLength;
                    skipDelay = true;
                }
            }
            while (true)
            {
                if (controls.Player.Interact.triggered)
                {
                    if(!skipDelay)
                        break;
                }
                else
                {
                    skipDelay = false;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    public void DropDialogue()
    {       
        dialogueGUI.SetActive(false);
        dialogueBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            dialogueGUI.SetActive(false);
            dialogueBoxGUI.gameObject.SetActive(false);
        }
    }
}
