using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaintPillar : MonoBehaviour
{
    public string requiredColor;
    public int maxCharge = 5;
    public int currentCharge = 0;
    public PuzzleManager connectedDoor;
    public TextMeshProUGUI chargeText;

    public void ReceivePaint(string color){
        if(color == requiredColor){
            currentCharge = Mathf.Min(currentCharge + 1, maxCharge);
            if(chargeText != null){
                chargeText.text = currentCharge + "/" + maxCharge;
            }

            Debug.Log("Pilar cargado:" + currentCharge + "/" + maxCharge);

            if(currentCharge >= maxCharge){
                connectedDoor.OpenDoor();
            }
        }
    }
}
