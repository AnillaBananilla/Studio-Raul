using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ButtonPuzzleManager : MonoBehaviour
{
    [Header("Secuencia de botones correcta")]
    public int[] correctSequence = {0,1,2,3};
    private int currentStep = 0;

    [Header("Sprites de puertas")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    [Header("Sonidos del puzzle")]
    public AudioSource audioSource;
    public AudioClip resetSound;
    public AudioClip successSound;

    [Header("Botones presionados hasta ahora")]
    public SequenceButton[] buttons;
    private List<SequenceButton> activeButtons = new List<SequenceButton>();

    public void OnButtonPressed(int buttonID){
        if(buttonID == correctSequence[currentStep]){
            SequenceButton b = GetButtonByID(buttonID);
            if(b != null){
                b.SetActiveVisual(true);
                activeButtons.Add(b);
            }

            currentStep++;
            if(currentStep >= correctSequence.Length){
                Debug.Log("Secuencia correcta completada");
                OpenDoor();
                if(successSound){
                    audioSource.PlayOneShot(successSound);
                }
            }
        }
        else{
            Debug.Log("Secuencia incorrecta, reiniciar");
            ResetSequence();
            if(resetSound){
                audioSource.PlayOneShot(resetSound);
            }
        }
    }

    private void OpenDoor(){
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);
    }

    public void ResetSequence(){
        currentStep = 0;
        foreach(var b in activeButtons){
            b.SetActiveVisual(false);
        }
        activeButtons.Clear();
    }
    private SequenceButton GetButtonByID(int id){
        foreach(var b in buttons){
            if(b.buttonID == id){
                return b;
            }
        }
        return null;
    }
}
