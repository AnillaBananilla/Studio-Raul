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

            if (inventoryItems.Count > 0)
            {
                selectedIndex = Mathf.Clamp(selectedIndex, 0, inventoryItems.Count - 1);
                MoveSelectionIndicator(selectedIndex);
            }
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
        string[] itemNames = { "Item1", "Item2", "Item3", "Item4", "Item5", "Item6" };
        int[] itemQuantities = { 5, 2, 0, 37, 1, 3 };

        for (int i = 0; i < itemNames.Length; i++)
        {
            AddItemToInventory(itemNames[i], itemQuantities[i]);
        }

        UpdateInventoryUI();
    }

    private void AddItemToInventory(string itemName, int quantity)
    {
        if (quantity <= 0) return;

        GameObject newButton = Instantiate(itemButtonPrefab, itemListContainer);
        Button button = newButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        GameObject quantityTextObj = new GameObject("QuantityText");
        TextMeshProUGUI quantityText = quantityTextObj.AddComponent<TextMeshProUGUI>();
        quantityText.transform.SetParent(newButton.transform);
        quantityText.rectTransform.anchoredPosition = new Vector2(50, 0);
        quantityText.fontSize = 20;
        quantityText.color = Color.white;

        if (buttonText != null)
        {
            buttonText.text = $"{itemName} x{quantity}";
        }

        int index = inventoryItems.Count;
        button.onClick.AddListener(() => EquipItem(index));

        InventoryItem newItem = new InventoryItem(itemName, quantity, button, buttonText, quantityText);
        inventoryItems.Add(newItem);
    }

    private void EquipItem(int index)
    {
        if (index >= 0 && index < inventoryItems.Count && inventoryItems[index].quantity > 0)
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

            if (item.quantity <= 0)
            {
                Destroy(item.button.gameObject);
                inventoryItems.RemoveAt(i);

                if (selectedIndex >= inventoryItems.Count)
                {
                    selectedIndex = Mathf.Max(0, inventoryItems.Count - 1);
                }
            }
            else
            {
                item.buttonText.text = item.quantity > 32
                    ? $"{item.name}"  // Si es infinito, no mostrar cantidad
                    : $"{item.name} x{item.quantity}"; // Mostrar cantidad normalmente

                item.quantityText.text = item.quantity > 32
                    ? "" // No mostrar texto de cantidad si es infinito
                    : "x" + item.quantity; // Mostrar cantidad si no es infinito
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

            if (equippedItem.quantity > 0)
            {
                // Si la cantidad es mayor a 32, no se reduce
                if (equippedItem.quantity > 32)
                {
                    Debug.Log("Used: " + equippedItem.name + " (infinite)");
                }
                else
                {
                    Debug.Log("Used: " + equippedItem.name);
                    equippedItem.quantity--;
                }

                UpdateInventoryUI();
            }
            else
            {
                Debug.Log("No item left to use.");
            }
        }
    }
}
