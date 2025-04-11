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
        inventoryUI.SetActive(isInventoryOpen);
        
        equipButton.onClick.AddListener(ToggleEquipItem);
        deleteButton.onClick.AddListener(DeleteItem);
        
        InitializeInventory();
    }

    private void ConfigureGrids()
    {
        // Configurar grid principal (2 columnas x 4 filas)
        mainInventoryGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        mainInventoryGrid.constraintCount = 4;
        mainInventoryGrid.cellSize = mainCellSize;

        // Configurar grid equipados (1 columna x 4 filas)
        equippedItemsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        equippedItemsGrid.constraintCount = 4;
        equippedItemsGrid.cellSize = equippedCellSize;

        // Configurar slot de consumible (1x1)
        consumableSlotGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        consumableSlotGrid.constraintCount = 1;
        consumableSlotGrid.cellSize = consumableCellSize;
    }

    private void Update()
    {
        if (consumableIndex >= 0 && inputHandler.useItem)
        {
            UseConsumableItem();
        }

        if (inputHandler.pressMenu) 
        {
            ToggleInventory();
            Debug.Log("Abrir menu");
        }
    }

    private void InitializeInventory()
    {
        ClearGrid(mainInventoryGrid.transform);
        ClearGrid(equippedItemsGrid.transform);
        ClearGrid(consumableSlotGrid.transform);
        itemButtons.Clear();
        equippedCount = 0;
        consumableIndex = -1;

        // Primero contar los items equipados
        foreach (var item in playerInventory.items)
        {
            if (item.isEquipped)
            {
                if (item.isConsumable)
                {
                    consumableIndex = playerInventory.items.IndexOf(item);
                }
                else
                {
                    equippedCount++;
                }
            }
        }

        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            // Saltar el item con nombre "NoItem" en el índice 0
            if (i == 0 && playerInventory.items[i].name == "NoItem") 
                continue;
                
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
    // Verificamos que la cantidad sea mayor que 0
    if (index >= 0 && index < playerInventory.items.Count && playerInventory.items[index].quantity > 0)
    {
        selectedIndex = index;
        MoveSelectionIndicator(selectedIndex);
        
        var item = playerInventory.items[selectedIndex];
        equipButton.interactable = true;
        deleteButton.interactable = true;
        equipButton.GetComponentInChildren<TextMeshProUGUI>().text = item.isEquipped ? "UNEQUIP" : "EQUIP";
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (selectedIndex >= 0) MoveSelectionIndicator(selectedIndex);
        }
        else
        {
            selectionIndicator.gameObject.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }

    public void ToggleEquipItem()
    {
        if (selectedIndex < 0 || selectedIndex >= playerInventory.items.Count)
            return;

        var item = playerInventory.items[selectedIndex];

        if (item.isEquipped)
        {
            // Si está equipado, lo desequipamos
            item.isEquipped = false;
            if (item.isConsumable)
            {
                consumableIndex = -1;
            }
            else
            {
                equippedCount--;
            }
            Debug.Log($"Unequipped: {item.name}");
        }
        else
        {
            // Verificar límites antes de equipar
            if (item.isConsumable)
            {
                if (consumableIndex >= 0)
                {
                    Debug.Log("Ya hay un consumible equipado");
                    return;
                }
                consumableIndex = selectedIndex;
            }
            else
            {
                if (equippedCount >= 4)
                {
                    Debug.Log("No puedes equipar más de 4 items");
                    return;
                }
                equippedCount++;
            }
            item.isEquipped = true;
            Debug.Log($"Equipped: {item.name}");
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

    // En lugar de eliminar el item, establecemos la cantidad a 0
    item.quantity = item.quantity-1;
    if(item.quantity == 0){
item.isEquipped = false; // También lo desequipamos por seguridad
selectedIndex = -1;
    selectionIndicator.gameObject.SetActive(false);
    equipButton.interactable = false;
    deleteButton.interactable = false;
    }
    

    // Actualizamos el inventario
    UpdateInventoryUI();
    
    // Deseleccionamos el item
    
}

    public void UpdateInventoryUI()
{
    ClearGrid(mainInventoryGrid.transform);
    ClearGrid(equippedItemsGrid.transform);
    ClearGrid(consumableSlotGrid.transform);
    itemButtons.Clear();

    // Mostrar items equipados primero
    for (int i = 0; i < playerInventory.items.Count; i++)
    {
        var item = playerInventory.items[i];
        
        // Saltar si es "NoItem" en índice 0 o si cantidad es 0
        if ((i == 0 && item.name == "NoItem") || item.quantity <= 0)
            continue;

        if (item.isEquipped)
        {
            if (item.isConsumable)
            {
                CreateItemInSlot(item, consumableSlotGrid.transform, i);
            }
            else
            {
                CreateEquippedItemButton(item, i);
            }
        }
    }

    // Mostrar items no equipados
    for (int i = 0; i < playerInventory.items.Count; i++)
    {
        var item = playerInventory.items[i];
        
        // Saltar si es "NoItem" en índice 0, si está equipado o si cantidad es 0
        if ((i == 0 && item.name == "NoItem") || item.isEquipped || item.quantity <= 0)
            continue;

        CreateItemInMainInventory(item, i);
    }

    MoveSelectionIndicator(selectedIndex);

    // Desactivar botones si el item seleccionado tiene cantidad 0
    if (selectedIndex >= 0 && selectedIndex < playerInventory.items.Count)
    {
        if (playerInventory.items[selectedIndex].quantity <= 0)
        {
            equipButton.interactable = false;
            deleteButton.interactable = false;
            selectedIndex = -1;
            selectionIndicator.gameObject.SetActive(false);
        }
    }
}

    private void CreateItemInMainInventory(PlayerInventory.InventoryItem item, int index)
    {
        GameObject newButton = Instantiate(itemButtonPrefab, mainInventoryGrid.transform);
        Button button = newButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = $"{item.name} x{item.quantity}";
        button.onClick.AddListener(() => SelectItem(index));

        itemButtons.Add(button);
    }

    private void CreateEquippedItemButton(PlayerInventory.InventoryItem item, int index)
    {
        GameObject newButton = Instantiate(itemButtonPrefab, equippedItemsGrid.transform);
        Button button = newButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = item.name;
        button.onClick.AddListener(() => SelectEquippedItem(index));

        itemButtons.Add(button);
    }

    private void CreateItemInSlot(PlayerInventory.InventoryItem item, Transform parent, int index)
    {
        var itemUI = Instantiate(itemButtonPrefab, parent);
        var buttonText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = item.isConsumable ? $"{item.name} x{item.quantity}" : item.name;

        Button button = itemUI.GetComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(() => SelectEquippedItem(index));
        itemButtons.Add(button);
    }

    private void MoveSelectionIndicator(int index)
    {
        if (index >= 0 && index < itemButtons.Count && itemButtons[index].gameObject.activeSelf)
        {
            //selectionIndicator.gameObject.SetActive(true);
            //selectionIndicator.position = itemButtons[index].transform.position;
        }
        else
        {
            //selectionIndicator.gameObject.SetActive(false);
        }
    }

    private void SelectEquippedItem(int index)
    {
        selectedIndex = index;
        var item = playerInventory.items[selectedIndex];
        equipButton.GetComponentInChildren<TextMeshProUGUI>().text = item.isEquipped ? "UNEQUIP" : "EQUIP";
        equipButton.interactable = true;
        deleteButton.interactable = true;
    }
}