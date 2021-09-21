using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueElement
{
    // 1 = text
    // 2 = yes/no
    // 3 = exchange of items
    // 4 = silence
    public int type;

    [Space]
    // text info
    public string text;

    [Space]
    // yes/no info
    public DialogueElement yesDialogue;
    public DialogueElement noDialogue;
    public DialogueElement silentDialogue;

    [Space]
    // exchange info
    // 0 = player receives
    // 1 = other receives
    // 2 = both exchange
    public int exchangeType;

    [Space]
    [Space]
    [Space]
    [Space]
    // silence info
    public float silenceTime;
}
