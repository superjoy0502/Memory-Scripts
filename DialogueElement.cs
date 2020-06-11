using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class DialogueElement
{
    public string name; // 캐릭터 이름
    public string[] txt; // 캐릭터 스토리 대사
    public string[] idletxt; // 캐릭터 노말 대사
}

[Serializable]
public class DialogueElements
{
    public DialogueElement[] elements;
}