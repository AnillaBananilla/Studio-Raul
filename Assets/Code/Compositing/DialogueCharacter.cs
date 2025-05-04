using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este es un Scriptable Object, que es para ver las emociones del personaje

[CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "Dialogue/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public bool isOnLeft = true;

    [System.Serializable]
    public class Emotion
    {
        public string emotionName;
        public Sprite portrait;
    }

    public List<Emotion> expressions = new List<Emotion>();

    public Sprite GetPortrait(string emotion)
    {
        foreach (Emotion e in expressions)
        {
            if (e.emotionName.ToLower() == emotion.ToLower())
                return e.portrait;
        }
        Debug.LogWarning($"Emotion '{emotion}' not found for {characterName}");
        return null;
    }
}