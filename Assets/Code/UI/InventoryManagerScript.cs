using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class InventoryManagerScripts : MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemListContainer;
    public GameObject itemButtonPrefab;
    public TextMeshProUGUI equippedItemText;
    public RectTransform selectionIndicator;

    public int infiniteThreshold = 33; // Variable pública para el umbral de infinito
    public PlayerInventory playerInventory; // Referencia al ScriptableObject del inventario

    private class InventoryItem
    {
        public string name;
        public int quantity;
        public Button button;
        public TextMeshProUGUI buttonText;
        public TextMeshProUGUI quantityText;

        public InventoryItem(string name, int quantity, Button button, TextMeshProUGUI buttonText, TextMeshProUGUI quantityText)
        {
            this.name = name;
            this.quantity = quantity;
            this.button = button;
            this.buttonText = buttonText;
            this.quantityText = quantityText;
        }
    }

    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    private int selectedIndex = 0;
    private bool isInventoryOpen = false;

    void Start()
    {
        inventoryUI.SetActive(false);
        AddItemToInventory("Sin Item", -1); // "Sin Item" siempre debe estar en el inventario
        PopulateInventory();
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        if (isInventoryOpen)
        {
            HandleKeyboardNavigation();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && !inventoryUI.activeSelf)
        {
            UseEquippedItem();
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
        Time.timeScale = isInventoryOpen ? 0f : 1f;

        if (isInventoryOpen)
        {
            UpdateInventoryUI();
            MoveSelectionIndicator(selectedIndex);
        }
    }

    private void HandleKeyboardNavigation()
    {
        if (inventoryItems.Count == 0)
        {
            equippedItemText.text = "No item selected";
            return;
        }

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            selectedIndex = (selectedIndex - 1 + inventoryItems.Count) % inventoryItems.Count;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            selectedIndex = (selectedIndex + 1) % inventoryItems.Count;
        }

        MoveSelectionIndicator(selectedIndex);

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
        {
            EquipItem(selectedIndex);
            ToggleInventory();
        }
    }

    private void PopulateInventory()
    {
        // Recorre la lista de ítems del ScriptableObject
        foreach (PlayerInventory.InventoryItem item in playerInventory.items)
        {
            AddItemToInventory(item.name, item.quantity);
        }

        UpdateInventoryUI();
    }

    private void AddItemToInventory(string itemName, int quantity)
    {
        if (quantity == 0 && itemName != "Sin Item") return; // Evita añadir items con cantidad 0

        GameObject newButton = Instantiate(itemButtonPrefab, itemListContainer);
        Button button = newButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        TextMeshProUGUI quantityText = new GameObject("QuantityText").AddComponent<TextMeshProUGUI>();
        quantityText.transform.SetParent(newButton.transform);
        quantityText.rectTransform.anchoredPosition = new Vector2(50, 0);
        quantityText.fontSize = 20;
        quantityText.color = Color.white;

        InventoryItem newItem = new InventoryItem(itemName, quantity, button, buttonText, quantityText);
        inventoryItems.Add(newItem);

        UpdateItemUI(newItem);
        button.onClick.AddListener(() => EquipItem(inventoryItems.IndexOf(newItem)));
    }

    private void UpdateItemUI(InventoryItem item)
    {
        item.buttonText.text = item.quantity > infiniteThreshold ? $"{item.name}" : $"{item.name} x{item.quantity}";
        item.quantityText.text = item.quantity > infiniteThreshold ? "" : "x" + item.quantity;
    }

    private void EquipItem(int index)
    {
        if (index >= 0 && index < inventoryItems.Count)
        {
            equippedItemText.text = "Equipped: " + inventoryItems[index].name;
            selectedIndex = index;
        }
        else
        {
            equippedItemText.text = "No item selected";
        }
    }

    private void MoveSelectionIndicator(int index)
    {
        if (inventoryItems.Count > 0 && selectionIndicator != null)
        {
            selectionIndicator.position = inventoryItems[index].button.transform.position;
        }
    }

    private void UpdateInventoryUI()
    {
        for (int i = inventoryItems.Count - 1; i >= 0; i--)
        {
            InventoryItem item = inventoryItems[i];

            if (item.quantity == 0 && item.name != "Sin Item")
            {
                Destroy(item.button.gameObject);
                inventoryItems.RemoveAt(i);

                if (selectedIndex >= inventoryItems.Count)
                {
                    selectedIndex = Mathf.Max(0, inventoryItems.Count - 1);
                }

                EquipItem(selectedIndex);
            }
            else
            {
                UpdateItemUI(item);
            }
        }

        if (inventoryItems.Count == 0)
        {
            equippedItemText.text = "No item selected";
        }
    }

    private void UseEquippedItem()
    {
        if (inventoryItems.Count > 0 && selectedIndex >= 0 && selectedIndex < inventoryItems.Count)
        {
            InventoryItem equippedItem = inventoryItems[selectedIndex];

            if (equippedItem.quantity > 0 || equippedItem.quantity > infiniteThreshold) // Ítems con cantidad > infiniteThreshold son infinitos
            {
                Debug.Log("Used: " + equippedItem.name);

                // Solo restar la cantidad si el ítem no es infinito
                if (equippedItem.quantity <= infiniteThreshold)
                {
                    equippedItem.quantity--;
                    playerInventory.items[selectedIndex].quantity = equippedItem.quantity; // Actualiza el ScriptableObject

                    if (equippedItem.quantity == 0)
                    {
                        EquipItem(0); // Equipar el primer ítem disponible
                    }
                }
                else
                {
                    Debug.Log("Ítem infinito usado: " + equippedItem.name);
                }

                UpdateInventoryUI();
            }
            else if (equippedItem.name == "Sin Item")
            {
                Debug.Log("Sin Item seleccionado, nada que usar.");
            }
        }
    }
}