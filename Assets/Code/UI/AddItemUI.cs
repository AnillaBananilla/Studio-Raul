using UnityEngine;

public class AddItemUI : MonoBehaviour
{
    public PlayerInventory playerInventory; 
    public InventoryManagerScripts inventoryManager; 
    public string itemNameToAdd; // Nombre del ítem que se añadirá al inventario
    public int quantityToAdd = 1; 
    private string playerTag = "Player"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entró al trigger tiene el tag del jugador
        if (other.CompareTag(playerTag))
        {
            if (playerInventory == null)
            {
                Debug.LogError("No se ha asignado el ScriptableObject del inventario del jugador en " + gameObject.name);
                return;
            }

            if (inventoryManager == null)
            {
                Debug.LogError("No se ha asignado el InventoryManagerScripts en " + gameObject.name);
                return;
            }

            // Busca el ítem en el inventario por su nombre
            PlayerInventory.InventoryItem item = playerInventory.items.Find(i => i.name == itemNameToAdd);

            if (item != null)
            {
                // Suma la cantidad especificada al ítem existente
                item.quantity += quantityToAdd;
                Debug.Log($"Se ha añadido {quantityToAdd} de {itemNameToAdd} al inventario. Nueva cantidad: {item.quantity}");
            }
            else
            {
                // Si el ítem no existe, podrías decidir crearlo y añadirlo.
                // Sin embargo, el script original asume que el ítem ya existe en el inventario.
                Debug.LogWarning($"El ítem con nombre {itemNameToAdd} no existe en el inventario.");
            }

            // Actualiza la UI del inventario
            inventoryManager.UpdateInventoryUI();

            // Opcional: Puedes destruir el objeto que contiene este script después de recoger el ítem
            Destroy(gameObject);
        }
    }
}