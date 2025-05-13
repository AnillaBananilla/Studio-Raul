using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "Dialogue/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public bool isOnLeft = true;

    [System.Serializable]
    public class Expression
    {
        public string emotion;
        public Sprite portrait;
    }

    public Expression[] expressions;

    public Sprite GetPortrait(string emotion)
    {
        foreach (var expr in expressions)
        {
            if (expr.emotion.ToLower() == emotion.ToLower())
                return expr.portrait;
        }

        Debug.LogWarning($"Emoción '{emotion}' no encontrada para {characterName}");
        return null;
    }
}
