using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class InventoryManagerScripts : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryUI;
    public GridLayoutGroup mainInventoryGrid; // Grid 2x4
    public GridLayoutGroup equippedItemsGrid; // Grid 1x4 (vertical)
    public GridLayoutGroup consumableSlotGrid; // Grid 1x1
    public GameObject itemButtonPrefab;
    public RectTransform selectionIndicator;

    [Header("Grid Settings")]
    public Vector2 mainCellSize = new Vector2(150, 150);
    public Vector2 equippedCellSize = new Vector2(120, 120);
    public Vector2 consumableCellSize = new Vector2(100, 100);

    [Header("Buttons")]
    public Button equipButton;
    public Button deleteButton;

    [Header("Inventory Data")]
    public PlayerInventory playerInventory;
    public InputHandler inputHandler;

    private List<Button> itemButtons = new List<Button>();
    private int selectedIndex = -1;
    private bool isInventoryOpen = false;
    private int equippedCount = 0;
    private int consumableIndex = -1;

    private void Start()
    {
        ConfigureGrids();
        inventoryUI.SetActive(false);
        
        equipButton.onClick.AddListener(ToggleEquipItem);
        deleteButton.onClick.AddListener(DeleteItem);
        
        InitializeInventory();
    }

    private void ConfigureGrids()
    {
        // Configurar grid principal (2 columnas x 4 filas)
        mainInventoryGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        mainInventoryGrid.constraintCount = 2;
        mainInventoryGrid.cellSize = mainCellSize;

        // Configurar grid equipados (1 columna x 4 filas)
        equippedItemsGrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        equippedItemsGrid.constraintCount = 4;
        equippedItemsGrid.cellSize = equippedCellSize;

        // Configurar slot de consumible (1x1)
        consumableSlotGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        consumableSlotGrid.constraintCount = 1;
        consumableSlotGrid.cellSize = consumableCellSize;
    }

    private void Update()
    {
        if (!isInventoryOpen) return;

        if (consumableIndex >= 0 && inputHandler.useItem)
        {
            UseConsumableItem();
        }
    }

    private void InitializeInventory()
    {
        ClearGrid(mainInventoryGrid.transform);
        ClearGrid(equippedItemsGrid.transform);
        ClearGrid(consumableSlotGrid.transform);
        itemButtons.Clear();

        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            AddItemButton(playerInventory.items[i], i);
        }

        UpdateInventoryUI();
    }

    private void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }

    private void AddItemButton(PlayerInventory.InventoryItem item, int index)
    {
        GameObject newButton = Instantiate(itemButtonPrefab, mainInventoryGrid.transform);
        Button button = newButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = $"{item.name} x{item.quantity}";
        button.onClick.AddListener(() => SelectItem(index));
        itemButtons.Add(button);
    }

    private void SelectItem(int index)
    {
        if (index >= 0 && index < playerInventory.items.Count && playerInventory.items[index].quantity > 0)
        {
            selectedIndex = index;
            MoveSelectionIndicator(selectedIndex);
            
            var item = playerInventory.items[selectedIndex];
            equipButton.interactable = true;
            deleteButton.interactable = true;
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
        Time.timeScale = isInventoryOpen ? 0f : 1f;

        if (isInventoryOpen)
        {
            UpdateInventoryUI();
            if (selectedIndex >= 0) MoveSelectionIndicator(selectedIndex);
        }
        else
        {
            selectionIndicator.gameObject.SetActive(false);
        }
    }

    public void ToggleEquipItem()
    {
        if (selectedIndex < 0 || selectedIndex >= playerInventory.items.Count)
            return;

        var item = playerInventory.items[selectedIndex];

        if (item.isConsumable)
        {
            // Manejar consumible
            if (consumableIndex >= 0)
            {
                playerInventory.items[consumableIndex].isEquipped = false;
            }
            
            consumableIndex = selectedIndex;
            item.isEquipped = true;
            Debug.Log($"Consumible equipado: {item.name}");
        }
        else
        {
            // Manejar equipables (con límite de 4)
            if (item.isEquipped)
            {
                item.isEquipped = false;
                equippedCount--;
                Debug.Log($"Desequipado: {item.name}");
            }
            else if (equippedCount < 4) // LÍMITE DE 4 EQUIPABLES
            {
                item.isEquipped = true;
                equippedCount++;
                Debug.Log($"Equipado: {item.name}");
            }
            else
            {
                Debug.Log("No puedes equipar más de 4 items");
                return;
            }
        }

        UpdateInventoryUI();
    }

    private void UseConsumableItem()
    {
        if (consumableIndex < 0 || consumableIndex >= playerInventory.items.Count)
            return;

        var item = playerInventory.items[consumableIndex];
        
        if (!item.isConsumable || item.quantity <= 0)
            return;

        ApplyConsumableEffect(item);
        item.quantity--;

        if (item.quantity <= 0)
        {
            item.isEquipped = false;
            consumableIndex = -1;
            Debug.Log("Consumible agotado");
        }

        UpdateInventoryUI();
    }

    private void ApplyConsumableEffect(PlayerInventory.InventoryItem item)
    {
        Debug.Log($"Efecto aplicado: {item.name}");
        // Implementar efectos específicos aquí
    }

    public void DeleteItem()
    {
        if (selectedIndex < 0 || selectedIndex >= playerInventory.items.Count)
            return;

        var item = playerInventory.items[selectedIndex];

        if (item.isEquipped)
        {
            if (item.isConsumable && consumableIndex == selectedIndex)
                consumableIndex = -1;
            else
                equippedCount--;
        }

        playerInventory.items.RemoveAt(selectedIndex);
        selectedIndex = -1;
        InitializeInventory();
    }

    public void UpdateInventoryUI()
    {
        ClearGrid(equippedItemsGrid.transform);
        ClearGrid(consumableSlotGrid.transform);

        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            var item = playerInventory.items[i];
            if (i < itemButtons.Count)
            {
                var button = itemButtons[i];
                var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = $"{item.name} x{item.quantity}";
                button.gameObject.SetActive(item.quantity > 0);

                if (item.isEquipped && item.quantity > 0)
                {
                    if (item.isConsumable)
                    {
                        CreateItemInSlot(item, consumableSlotGrid.transform);
                    }
                    else if (equippedCount <= 4) // Verificación adicional
                    {
                        CreateItemInSlot(item, equippedItemsGrid.transform);
                    }
                }
            }
        }

        MoveSelectionIndicator(selectedIndex);
    }

    private void CreateItemInSlot(PlayerInventory.InventoryItem item, Transform parent)
    {
        var itemUI = Instantiate(itemButtonPrefab, parent);
        var buttonText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = item.isConsumable ? $"{item.name} x{item.quantity}" : item.name;
        itemUI.GetComponent<Button>().interactable = false;
    }

    private void MoveSelectionIndicator(int index)
    {
        if (index >= 0 && index < itemButtons.Count && itemButtons[index].gameObject.activeSelf)
        {
            selectionIndicator.gameObject.SetActive(true);
            selectionIndicator.position = itemButtons[index].transform.position;
        }
        else
        {
            selectionIndicator.gameObject.SetActive(false);
        }
    }
}