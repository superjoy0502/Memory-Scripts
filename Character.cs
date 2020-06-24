using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    public string jsonName;
    
    // public TextAsset jsonText;
    
    public string path;
    public DialogueElements dialogues;

    public RectTransform panel;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI textTxt;
    
    public bool dialogueActive;
    public int posNum; // DialogueElements 안의 진행도
    public int txtNum; // DialogueElement 안의 진행도

    public AudioSource voice;

    private void Awake()
    {
        // jsonText = Resources.Load<TextAsset>("Local/" + jsonName);
        // path = Directory.GetCurrentDirectory() + "\\Assets\\Local\\" + jsonName + ".json";
        path = Application.dataPath + "/StreamingAssets" + "/Local/" + jsonName + ".json";
    }

    private void Start()
    {
        // Debug.Log(jsonText.text);
        dialogues = new DialogueElements();
        Debug.Log(File.ReadAllText(path));
        // dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + jsonText.text + "}");
        dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + File.ReadAllText(path) + "}");
        // dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>(File.ReadAllText(path));

        txtNum = 0;
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
        else
        {
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                StartDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        StartCoroutine(DisplayDialogue());
    }

    private void NextDialogue()
    {
        // 남은 대사 수가 0이 아닐때
        if (txtNum != dialogues.elements[posNum].txt.Length-1)
        {
            txtNum += 1;
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            if (dialogues.elements[posNum].end) // 대사의 end 값이 true 일때
            {
                dialogueActive = false;
                panel.gameObject.SetActive(false); // 대화를 끝낸다
            }
            else // 대사의 end 값이 false 일때
            {
                txtNum = 0;
                posNum += 1;
                StartDialogue(); // 다음 대화를 시작한다
            }
        }
    }

    private IEnumerator DisplayDialogue()
    {
        nameTxt.text = "";
        textTxt.text = ""; // 값 리셋
        nameTxt.text = dialogues.elements[posNum].name;
        // textTxt.text = dialogues.elements[posNum].txt[txtNum];
        for (int i = 0; i < dialogues.elements[posNum].txt[txtNum].Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            textTxt.text += dialogues.elements[posNum].txt[txtNum][i].ToString();
            if (voice) { voice.Play(); }
        }
    }
}