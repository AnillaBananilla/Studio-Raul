using UnityEngine;
using UnityEngine.UI;

public class SO_InventoryTesting : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public InventoryManagerScripts inventoryManager;
    
    [Header("Item to Add")]
    public string itemID;
    public string itemName;
    public Sprite icon;
    public bool isConsumable;
    public int initialQuantity = 1;
    public int attackModifier;
    public int defenseModifier;
    public int healthModifier;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(AddTestItem);
        }
        else
        {
            Debug.LogError("Este script debe estar adjunto a un objeto con un componente Button.");
        }
    }

    private void AddTestItem()
    {
        if (playerInventory == null || inventoryManager == null)
        {
            Debug.LogError("Faltan referencias al inventario o manager");
            return;
        }

        PlayerInventory.InventoryItem newItem = new PlayerInventory.InventoryItem
        {
            itemID = this.itemID,
            itemName = this.itemName,
            quantity = this.initialQuantity,
            isConsumable = this.isConsumable,
            icon = this.icon,
            attackModifier = this.attackModifier,
            defenseModifier = this.defenseModifier,
            healthModifier = this.healthModifier
        };

        if (playerInventory.AddItem(newItem))
        {
            Debug.Log($"Item {itemName} añadido al inventario");
            if (inventoryManager.isInventoryOpen)
            {
                inventoryManager.InitializeInventoryUI();
            }
        }
        else
        {
            Debug.LogWarning("No se pudo añadir el item. Inventario lleno.");
        }
    }
}