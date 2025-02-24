using UnityEngine;
using UnityEngine.UI;

public class SO_InventoryTesting : MonoBehaviour
{
    public PlayerInventory playerInventory; // Referencia al ScriptableObject
    public InventoryManagerScripts inventoryManager; // Referencia al InventoryManagerScripts
    public string itemName; // Nombre del �tem al que quieres sumar +1

    private Button button;

    void Start()
    {
        // Obt�n el componente Button
        button = GetComponent<Button>();

        // Asigna la funci�n al evento onClick del bot�n
        if (button != null)
        {
            button.onClick.AddListener(AddItem);
        }
        else
        {
            Debug.LogError("Este script debe estar adjunto a un objeto con un componente Button.");
        }
    }

    private void AddItem()
    {
        if (playerInventory == null)
        {
            Debug.LogError("No se ha asignado un ScriptableObject de inventario.");
            return;
        }

        if (inventoryManager == null)
        {
            Debug.LogError("No se ha asignado un InventoryManagerScripts.");
            return;
        }

        // Busca el �tem en el inventario por su nombre
        PlayerInventory.InventoryItem item = playerInventory.items.Find(i => i.name == itemName);

        if (item != null)
        {
            // Suma +1 a la cantidad del �tem en el ScriptableObject
            item.quantity++;
            Debug.Log($"Se ha sumado +1 a {itemName}. Nueva cantidad: {item.quantity}");

            // Sincroniza la cantidad con el InventoryManagerScripts
            //inventoryManager.AddItem(itemName, 1);
        }
        else
        {
            Debug.LogWarning($"No se encontr� el �tem con nombre {itemName} en el inventario.");
        }
    }
}