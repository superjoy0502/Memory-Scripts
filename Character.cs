using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    public string jsonName; // JSON 파일 이름
    
    public TextAsset jsonText;
    
    public string path;
    public DialogueElements dialogues; // 대화 저장소

    public RectTransform panel;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI textTxt;
    
    public bool dialogueActive;
    public int posNum; // 대화 진행도 (에디터 상에서 편집)
    [HideInInspector]
    public int txtNum; // 대사 진행도

    public AudioSource voice;

    private void Awake()
    {
        // JSON 불러올 준비
        path = Application.dataPath + "/StreamingAssets" + "/Local/" + jsonName + ".json"; // 유저 패치가 있을 수도 있는 버젼의 주소
        jsonText = Resources.Load<TextAsset>("Local/" + jsonName); // 원본 파일 (리소스)
    }

    private void Start()
    {
        
        // 불러온 JSON 저장
        
        dialogues = new DialogueElements(); // 객체 생성
        
        string readText = File.ReadAllText(path); // 주소에서 JSON 읽기
        dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + readText + "}"); // 객체에다 읽은 JSON 넣기
        
        // dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + jsonText.text + "}"); // 리소스를 이용해 대화를 표시하는 방법. 안 씀.

        if (jsonText.text != readText)
        {
            // TODO UI로 패치가 공식이 아니라는 것을 유저에게 알려주기
        }
        
        txtNum = 0; // 대사 진행도 초기화
    }

    private void Update()
    {
        if (dialogueActive) // 대화중일때
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
            {
                NextDialogue(); // 다음으로 넘어가기
            }
        }
        else // 초기 테스트용 코드
        {
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                StartDialogue();
            }
        }
    }

    public void StartDialogue() // 대화 시작하기
    {
        dialogueActive = true; // 대화 진행중 상태로 변경
        StartCoroutine(DisplayDialogue()); // 대사 출력 시작
    }

    private IEnumerator DisplayDialogue() // 대사 출력
    {
        nameTxt.text = "";
        textTxt.text = ""; // 값 리셋
        nameTxt.text = dialogues.elements[posNum].name; // 이름 출력
        // textTxt.text = dialogues.elements[posNum].txt[txtNum]; // 내용 '즉시' 출력
        for (int i = 0; i < dialogues.elements[posNum].txt[txtNum].Length; i++) // 내용을 차례대로 출력
        {
            yield return new WaitForSeconds(0.05f); // 뜸 주고
            textTxt.text += dialogues.elements[posNum].txt[txtNum][i].ToString(); // 한글자 출력하고
            if (voice) { voice.Play(); } // 목소리 있으면 재생하고
        }
    }

    private void NextDialogue() // 다음으로 넘어가기
    {
        if (txtNum != dialogues.elements[posNum].txt.Length-1) // 대사가 남아 있을 때
        {
            txtNum += 1; // 대사 진행도 1 올리고
            StartCoroutine(DisplayDialogue()); // 대사 출력
        }
        else // 대사가 더 없을 때
        {
            if (dialogues.elements[posNum].end || posNum == dialogues.elements.Length-1) // 대사의 end 값이 true 일때, 또는 대화의 끝에 도달했을 때
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
}