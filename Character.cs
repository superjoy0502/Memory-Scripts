using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    public string jsonname;

    public string path;
    public DialogueElements dialogues;

    public RectTransform panel;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI textTxt;
    
    public bool dialogueActive;
    public int posNum;
    public int txtNum;

    private void Awake()
    {
        path = Directory.GetCurrentDirectory() + "\\Assets\\Local\\" + jsonname + ".json";
    }

    private void Start()
    {
        dialogues = new DialogueElements();
        dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + File.ReadAllText(path) + "}");

        txtNum = 0;
        
        StartDialogue();
    }

    private void Update()
    {
        if (dialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        nameTxt.text = dialogues.elements[posNum].name;
        textTxt.text = dialogues.elements[posNum].txt[txtNum];
    }

    public void NextDialogue()
    {
        if (txtNum != dialogues.elements[posNum].txt.Length-1)
        {
            txtNum += 1;
            nameTxt.text = dialogues.elements[posNum].name;
            textTxt.text = dialogues.elements[posNum].txt[txtNum];
        }
        else
        {
            if (dialogues.elements[posNum].end)
            {
                dialogueActive = false;
                panel.gameObject.SetActive(false);
            }
            else
            {
                txtNum = 0;
                posNum += 1;
                StartDialogue();
            }
        }
    }
}