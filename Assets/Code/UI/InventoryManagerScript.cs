using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class InventoryManagerScripts : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryUI;
    public GridLayoutGroup mainInventoryGrid;
    public GridLayoutGroup equippedItemsGrid;
    public GridLayoutGroup consumableSlotGrid;
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

    private List<Button> mainInventoryButtons = new List<Button>();
    private List<Button> equippedItemsButtons = new List<Button>();
    private Button consumableButton;
    private int selectedIndex = -1;
    public bool isInventoryOpen = false;
    private bool isSelectedFromMain = true;

    private void Start()
    {
        ConfigureGrids();
        inventoryUI.SetActive(false);
        
        equipButton.onClick.AddListener(ToggleEquipItem);
        deleteButton.onClick.AddListener(DeleteItem);
        
        InitializeInventoryUI();
    }

    private void ConfigureGrids()
    {
        mainInventoryGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        mainInventoryGrid.constraintCount = 2;
        mainInventoryGrid.cellSize = mainCellSize;

        equippedItemsGrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        equippedItemsGrid.constraintCount = 4;
        equippedItemsGrid.cellSize = equippedCellSize;

        consumableSlotGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        consumableSlotGrid.constraintCount = 1;
        consumableSlotGrid.cellSize = consumableCellSize;
    }

    private void Update()
    {
<<<<<<< HEAD
        if (inputHandler.pressMenu)
        {
            ToggleInventory();
        }
=======
        if (!isInventoryOpen) return;
>>>>>>> parent of 0a308b1 (UI OpenFix, now its time to item)

        // Uso de consumible directamente con inputHandler.useItem
        if (inputHandler.useItem && playerInventory.equippedConsumable != null)
        {
            UseConsumableItem();
        }
    }
<<<<<<< HEAD
=======

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
>>>>>>> parent of 0a308b1 (UI OpenFix, now its time to item)

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
        Time.timeScale = isInventoryOpen ? 0f : 1f;

        if (isInventoryOpen)
        {
            InitializeInventoryUI();
        }
        else
        {
            selectedIndex = -1;
            selectionIndicator.gameObject.SetActive(false);
        }
    }

    public void InitializeInventoryUI()
    {
        ClearAllUI();
        CreateMainInventoryUI();
        CreateEquippedItemsUI();
        CreateConsumableSlotUI();
        UpdateButtonStates();
    }

    private void ClearAllUI()
    {
        foreach (Transform child in mainInventoryGrid.transform)
            Destroy(child.gameObject);
        
        foreach (Transform child in equippedItemsGrid.transform)
            Destroy(child.gameObject);
        
        if (consumableSlotGrid.transform.childCount > 0)
            Destroy(consumableSlotGrid.transform.GetChild(0).gameObject);

        mainInventoryButtons.Clear();
        equippedItemsButtons.Clear();
        consumableButton = null;
    }

    private void CreateMainInventoryUI()
    {
        for (int i = 0; i < playerInventory.mainItems.Count; i++)
        {
            CreateItemButton(playerInventory.mainItems[i], i, mainInventoryGrid.transform, 
                          mainInventoryButtons, () => SelectItem(i, true));
        }
    }

    private void CreateEquippedItemsUI()
    {
        for (int i = 0; i < playerInventory.equippedItems.Count; i++)
        {
            CreateItemButton(playerInventory.equippedItems[i], i, equippedItemsGrid.transform, 
                          equippedItemsButtons, () => SelectItem(i, false));
        }
    }

    private void CreateConsumableSlotUI()
    {
        if (playerInventory.equippedConsumable != null)
        {
            var buttonObj = Instantiate(itemButtonPrefab, consumableSlotGrid.transform);
            consumableButton = buttonObj.GetComponent<Button>();
            UpdateButtonUI(consumableButton, playerInventory.equippedConsumable);
            consumableButton.onClick.AddListener(() => SelectConsumableItem());
        }
    }

    private void CreateItemButton(PlayerInventory.InventoryItem item, int index, Transform parent, 
                               List<Button> buttonList, UnityEngine.Events.UnityAction onClickAction)
    {
        var buttonObj = Instantiate(itemButtonPrefab, parent);
        Button button = buttonObj.GetComponent<Button>();
        UpdateButtonUI(button, item);
        button.onClick.AddListener(onClickAction);
        buttonList.Add(button);
    }

    private void UpdateButtonUI(Button button, PlayerInventory.InventoryItem item)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        Image iconImage = button.transform.Find("Icon").GetComponent<Image>();
        
        buttonText.text = $"{item.itemName} x{item.quantity}";
        iconImage.sprite = item.icon;
        iconImage.enabled = item.icon != null;
    }

    private void SelectItem(int index, bool fromMainInventory)
    {
        selectedIndex = index;
        isSelectedFromMain = fromMainInventory;
        
        if (fromMainInventory)
            MoveSelectionIndicator(index, mainInventoryButtons);
        else
            MoveSelectionIndicator(index, equippedItemsButtons);

        UpdateButtonStates();
    }

    private void SelectConsumableItem()
    {
        selectedIndex = -1; // Índice especial para consumible
        isSelectedFromMain = false;
        MoveSelectionIndicator(0, new List<Button>{consumableButton});
        UpdateButtonStates();
    }

    private void MoveSelectionIndicator(int index, List<Button> buttons)
    {
        if (index >= 0 && index < buttons.Count && buttons[index] != null)
        {
            selectionIndicator.gameObject.SetActive(true);
            selectionIndicator.position = buttons[index].transform.position;
        }
        else
        {
            selectionIndicator.gameObject.SetActive(false);
        }
    }

<<<<<<< HEAD
    private void UpdateButtonStates()
    {
        bool hasSelection = selectedIndex != -1 || playerInventory.equippedConsumable != null;
=======
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
>>>>>>> parent of 0a308b1 (UI OpenFix, now its time to item)
        
        // Configurar botón Equipar/Desequipar
        if (isSelectedFromMain && selectedIndex >= 0 && selectedIndex < playerInventory.mainItems.Count)
        {
            equipButton.interactable = true;
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = 
                playerInventory.mainItems[selectedIndex].isConsumable ? "Usar" : "Equipar";
        }
        else if (!isSelectedFromMain && (selectedIndex >= 0 || playerInventory.equippedConsumable != null))
        {
            equipButton.interactable = true;
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Desequipar";
        }
        else
        {
            equipButton.interactable = false;
        }
        
        // Configurar botón Eliminar
        deleteButton.interactable = hasSelection;
    }

    public void ToggleEquipItem()
    {
        if (isSelectedFromMain && selectedIndex >= 0 && selectedIndex < playerInventory.mainItems.Count)
        {
            // Equipar o usar desde inventario principal
            if (playerInventory.mainItems[selectedIndex].isConsumable)
            {
                // Si es consumible, equiparlo (que ocupará el slot de consumible)
                if (playerInventory.EquipItem(selectedIndex))
                {
                    Debug.Log("Consumible equipado para uso");
                }
            }
            else
            {
                // Si no es consumible, equiparlo normalmente
                if (playerInventory.EquipItem(selectedIndex))
                {
                    Debug.Log("Ítem equipado exitosamente");
                }
            }
        }
        else
        {
            // Desequipar
            bool isConsumable = (selectedIndex == -1 && playerInventory.equippedConsumable != null);
            int actualIndex = isConsumable ? 0 : selectedIndex;
            
            if (playerInventory.UnequipItem(actualIndex, isConsumable))
            {
                Debug.Log("Ítem desequipado exitosamente");
            }
        }

        InitializeInventoryUI();
    }

    public void UseConsumableItem()
    {
        if (playerInventory.equippedConsumable == null) return;

        // Aplicar efectos del consumible
        ApplyItemEffects(playerInventory.equippedConsumable);
        
        // Reducir cantidad
        playerInventory.equippedConsumable.quantity--;

        // Si se acabó, removerlo
        if (playerInventory.equippedConsumable.quantity <= 0)
        {
            playerInventory.equippedConsumable = null;
        }

        InitializeInventoryUI();
    }

    private void ApplyItemEffects(PlayerInventory.InventoryItem item)
    {
        Debug.Log($"Usando consumible: {item.itemName}");
        // Implementar efectos reales aquí
        // Ejemplo: playerStats.health += item.healthModifier;
    }

    public void DeleteItem()
    {
        if (isSelectedFromMain && selectedIndex >= 0 && selectedIndex < playerInventory.mainItems.Count)
        {
            playerInventory.mainItems.RemoveAt(selectedIndex);
        }
        else if (!isSelectedFromMain)
        {
            if (selectedIndex >= 0 && selectedIndex < playerInventory.equippedItems.Count)
            {
                playerInventory.equippedItems.RemoveAt(selectedIndex);
            }
            else if (playerInventory.equippedConsumable != null)
            {
                playerInventory.equippedConsumable = null;
            }
        }

        InitializeInventoryUI();
    }
<<<<<<< HEAD
=======

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
>>>>>>> parent of 0a308b1 (UI OpenFix, now its time to item)
}