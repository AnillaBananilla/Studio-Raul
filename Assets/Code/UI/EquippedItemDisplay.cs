using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquippedItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI equippedItemText; // Texto que mostrará el ítem equipado
    public Image equippedItemImage; // Imagen que cambiará de color
    private string itemName;

    private void Update()
    {
        itemName = equippedItemText.text;
        if (itemName.Contains("Item1"))
            equippedItemImage.color = Color.green;
        else if (itemName.Contains("Item2"))
            equippedItemImage.color = Color.red;
        else if (itemName.Contains("Item3"))
            equippedItemImage.color = Color.cyan;
        else if (itemName.Contains("Item4"))
            equippedItemImage.color = Color.magenta;
        else if (itemName.Contains("Item5"))
            equippedItemImage.color = Color.yellow;
        else if (itemName.Contains("Item6"))
            equippedItemImage.color = Color.gray;
        else
            equippedItemImage.color = Color.white;
    }
}
