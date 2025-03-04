using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
/*    [SerializeField] */GameObject dialogueBox;
/*    [SerializeField] */TextMeshProUGUI dialogueText;
    [SerializeField] int lettersPerSecond;

    Dialogue dialogue;
    int currentLine = 0;
    private bool isTyping;
    public bool CURRENTTPYINGSTATUS;

    public event Action OnShowText;
    public event Action OnShowTextAndStartTest;
    public event Action OnCloseText;

    public static DialogueManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
        if (dialogueBox == null)
        {
            dialogueBox = GameObject.FindWithTag("DialogueBox");
            dialogueText = dialogueBox.transform.Find("DialogueTEXT").GetComponent<TextMeshProUGUI>();
        }
        //DontDestroyOnLoad(Instance);
        DontDestroyOnLoad(gameObject);
    }

    //used to show the first line of dialogue
    public IEnumerator ShowDialogue(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        currentLine = 0;

        //send event
        OnShowText?.Invoke();
        CURRENTTPYINGSTATUS = true;

        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
        //Debug.Log("First line drawn");
    }

    public IEnumerator ShowDialogueAndLoadTest(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        currentLine = 0;

        //send event
        OnShowTextAndStartTest?.Invoke();
        CURRENTTPYINGSTATUS = true;

        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
        //Debug.Log("First line drawn");
    }

    //types out dialogue within array
    public IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1.0f / lettersPerSecond);
        }

        isTyping = false;
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            currentLine++;

            //if current line is less than # of lines in dialogue
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                //otherwise close the dialogue box
                currentLine = 0;
                dialogueBox.SetActive(false);
                CURRENTTPYINGSTATUS = false;
                OnCloseText?.Invoke();
            }
        }
    }

    public void HandleUpdateAndLoadTest()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            currentLine++;

            //if current line is less than # of lines in dialogue
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                //otherwise close the dialogue box
                currentLine = 0;
                dialogueBox.SetActive(false);
                CURRENTTPYINGSTATUS = false;
                OnCloseText?.Invoke();
                TestManager.Instance.InitTest();
            }
        }
    }

    public void ResetCurrentLine()
    {
        currentLine = 0;
    }
}
