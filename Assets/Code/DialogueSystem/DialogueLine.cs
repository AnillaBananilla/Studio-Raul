using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    public string emotion = "Neutral";

    [TextArea(3, 5)]
    public string lineText;
    public string panelStyle = "Texto_Neutral";

    public bool isMovementEvent;
    public Vector2 moveToPosition;
    public bool waitForMovement;

    public bool isChoicePoint;
    public string[] choices;
    public int[] jumpToLine;
}
