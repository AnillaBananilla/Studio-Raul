using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Inventory/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    [System.Serializable]
    public class InventoryItem
    {
        public string name;
        public int quantity;
        public bool isConsumable; // Si es `false`, será equipable
        public bool isEquipped;   // Solo para equipables
    }
}
