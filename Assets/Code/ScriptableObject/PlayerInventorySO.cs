using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPlayerInventory", menuName = "Inventory/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemID;
        public string itemName;
        public int quantity;
        public bool isConsumable;
        public Sprite icon;
        // Stats que afecta cuando está equipado
        public int attackModifier;
        public int defenseModifier;
        public int healthModifier;
    }

    public List<InventoryItem> mainItems = new List<InventoryItem>();    // Máximo 8 slots
    public List<InventoryItem> equippedItems = new List<InventoryItem>(); // Máximo 4 slots
    public InventoryItem equippedConsumable;                             // 1 slot

    // Añade un nuevo ítem al inventario principal
    public bool AddItem(InventoryItem newItem)
    {
        // Verificar si ya existe un ítem igual en el inventario principal
        InventoryItem existingItem = mainItems.Find(i => i.itemID == newItem.itemID);

        if (existingItem != null)
        {
            existingItem.quantity += newItem.quantity;
            return true;
        }
        else if (mainItems.Count < 8)
        {
            mainItems.Add(newItem);
            return true;
        }

        Debug.LogWarning("Inventario principal lleno. No se puede añadir más ítems.");
        return false;
    }

    // Elimina un ítem del inventario
    public void RemoveItem(string itemID, int quantity = 1)
    {
        // Buscar en el inventario principal
        InventoryItem item = mainItems.Find(i => i.itemID == itemID);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
                mainItems.Remove(item);
            return;
        }

        // Buscar en ítems equipados
        item = equippedItems.Find(i => i.itemID == itemID);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
                equippedItems.Remove(item);
            return;
        }

        // Buscar en consumible equipado
        if (equippedConsumable != null && equippedConsumable.itemID == itemID)
        {
            equippedConsumable.quantity -= quantity;
            if (equippedConsumable.quantity <= 0)
                equippedConsumable = null;
        }
    }

    // Mueve un ítem del inventario principal al equipado
    public bool EquipItem(int mainInventoryIndex)
    {
        if (mainInventoryIndex < 0 || mainInventoryIndex >= mainItems.Count)
            return false;

        InventoryItem itemToEquip = mainItems[mainInventoryIndex];

        if (itemToEquip.isConsumable)
        {
            if (equippedConsumable == null)
            {
                equippedConsumable = itemToEquip;
                mainItems.RemoveAt(mainInventoryIndex);
                return true;
            }
            return false;
        }
        else if (equippedItems.Count < 4)
        {
            equippedItems.Add(itemToEquip);
            mainItems.RemoveAt(mainInventoryIndex);
            return true;
        }

        return false;
    }

    // Mueve un ítem equipado de vuelta al inventario principal
    public bool UnequipItem(int equippedIndex, bool isConsumableSlot = false)
    {
        if (isConsumableSlot)
        {
            if (equippedConsumable != null && mainItems.Count < 8)
            {
                mainItems.Add(equippedConsumable);
                equippedConsumable = null;
                return true;
            }
            return false;
        }
        else if (equippedIndex >= 0 && equippedIndex < equippedItems.Count && mainItems.Count < 8)
        {
            mainItems.Add(equippedItems[equippedIndex]);
            equippedItems.RemoveAt(equippedIndex);
            return true;
        }

        return false;
    }
}