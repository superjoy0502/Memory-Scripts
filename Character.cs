using System;
using System.IO;
using UnityEngine;

public class Character : MonoBehaviour
{

    public string jsonname;

    public string path;
    public DialogueElements dialogues;

    private void Awake()
    {
        path = Directory.GetCurrentDirectory() + "\\Assets\\Local\\" + jsonname + ".json";
    }

    private void Start()
    {
        dialogues = new DialogueElements();
        Debug.Log(File.ReadAllText(path));
        dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + File.ReadAllText(path) + "}");

        #region Convert2Json (Test)
        // string[] hi = {"Hello", "World!"};
        // string[] idlehi = {"Hello", "Idle!"};
        //
        // dialogue.name = "John";
        // dialogue.txt = hi;
        // dialogue.idletxt = idlehi;
        //
        // Debug.Log(JsonUtility.ToJson(dialogue));
        #endregion

        Debug.Log(dialogues.elements[1].txt[0]);
    }
}