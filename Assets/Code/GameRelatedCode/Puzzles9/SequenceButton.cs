using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceButton : MonoBehaviour
{
    public int buttonID;
    public ButtonPuzzleManager puzzleManager;

    public AudioSource audioSource;
    public AudioClip buttonSound;

    private SpriteRenderer sr;
    private Color originalColor;
    public Color activeColor = Color.green;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void PressButton(){
        puzzleManager.OnButtonPressed(buttonID);
        if(audioSource && buttonSound){
            audioSource.pitch = 1.0f + 0.1f * buttonID;
            audioSource.PlayOneShot(buttonSound);
        }
    }

    public void SetActiveVisual(bool isActive){
        if(sr != null){
            sr.color = isActive ? activeColor : originalColor;
        }
    }
}
