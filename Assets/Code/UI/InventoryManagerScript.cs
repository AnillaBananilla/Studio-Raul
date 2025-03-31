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
        inventoryUI.SetActive(isInventoryOpen);
        
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
        if (inputHandler.pressMenu)
        {
            ToggleInventory();
        }

        // Uso de consumible directamente con inputHandler.useItem
        if (inputHandler.useItem && playerInventory.equippedConsumable != null)
        {
            UseConsumableItem();
        }
    }

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

    private void UpdateButtonStates()
    {
        bool hasSelection = selectedIndex != -1 || playerInventory.equippedConsumable != null;
        
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
}