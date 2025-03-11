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

    public int infiniteThreshold = 33; // Variable p�blica para el umbral de infinito
    public PlayerInventory playerInventory; // Referencia al ScriptableObject del inventario

    public PlayerMovement playerMovement;

    private List<Button> itemButtons = new List<Button>(); // Lista para mantener referencias a los botones
    private int selectedIndex = 0;
    private bool isInventoryOpen = false;
    private string currentlyEquippedItem = "";

    public TextMeshProUGUI equippedItemDisplayText;

    void Start()
    {
        inventoryUI.SetActive(false);
        PopulateInventory();
    }

    void Update()
{
    if (playerMovement.pressMenu) 
    {
        ToggleInventory();
    }

    if (isInventoryOpen)
    {
        HandleKeyboardNavigation();
    }

    if (playerMovement.pressEquip && !inventoryUI.activeSelf) 
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
    else
    {
        selectionIndicator.gameObject.SetActive(false); // Oculta el indicador cuando se cierra el inventario
    }
}

    private void HandleKeyboardNavigation()
    {
        if (playerInventory.items.Count == 0)
        {
            equippedItemText.text = "No item selected";
            return;
        }
            //Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame || 
        if (playerMovement.isNavigatingLeft)
        {
            // Navegacion hacia la izquierda, evitando el Index 0
            selectedIndex = (selectedIndex - 1 + playerInventory.items.Count) % playerInventory.items.Count;
            if (selectedIndex == 0) selectedIndex = playerInventory.items.Count - 1; // Evitar Index 0
        }
        //Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame || 
        else if (playerMovement.isNavigatingRight)
        {
            // Navegacion hacia la derecha, evitando el Index 0
            selectedIndex = (selectedIndex + 1) % playerInventory.items.Count;
            if (selectedIndex == 0) selectedIndex = 1; // Evitar Index 0
        }

        MoveSelectionIndicator(selectedIndex);
        //Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame || 
        if (playerMovement.isSelecting) 
        {
            EquipItem(selectedIndex);
            //ToggleInventory();
        }
    }

    private void PopulateInventory()
    {
        // Limpia los botones existentes
        foreach (var button in itemButtons)
        {
            Destroy(button.gameObject);
        }
        itemButtons.Clear();

        // Crea botones para cada �tem en el ScriptableObject
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            AddItemButton(playerInventory.items[i], i);
        }

        UpdateInventoryUI();
    }

    private void AddItemButton(PlayerInventory.InventoryItem item, int index)
    {
        GameObject newButton = Instantiate(itemButtonPrefab, itemListContainer);
        Button button = newButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        /*
        TextMeshProUGUI quantityText = new GameObject("QuantityText").AddComponent<TextMeshProUGUI>();
        quantityText.transform.SetParent(newButton.transform);
        quantityText.rectTransform.anchoredPosition = new Vector2(50, 0);
        quantityText.fontSize = 20;
        quantityText.color = Color.white;

        buttonText.text = item.name;
        quantityText.text = item.quantity > infiniteThreshold ? "" : "x" + item.quantity;
        */
        button.onClick.AddListener(() => EquipItem(index));
        itemButtons.Add(button);
    }

    public void UpdateInventoryUI()
{
    for (int i = 0; i < playerInventory.items.Count; i++)
    {
        var item = playerInventory.items[i];
        var button = itemButtons[i];
        var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        var quantityText = button.transform.Find("QuantityText")?.GetComponent<TextMeshProUGUI>();

        if (item.quantity == 0 && item.name != "No Item")
        {
            button.gameObject.SetActive(false); // Desactiva el botón si la cantidad es 0

            // Si el ítem seleccionado se agotó, regresar al índice 0
            if (selectedIndex == i)
            {
                selectedIndex = 0; // Regresar al índice 0
                MoveSelectionIndicator(selectedIndex); // Mover el indicador de selección
            }
        }
        else
        {
            button.gameObject.SetActive(true); // Reactiva el botón si la cantidad es mayor que 0
            buttonText.text = item.name;
            if (quantityText != null)
            {
                quantityText.text = item.quantity > infiniteThreshold ? "" : "x" + item.quantity;
            }
        }
    }

    // Si no hay ítems disponibles, ocultar el indicador de selección
    if (playerInventory.items.Count == 0 || selectedIndex >= playerInventory.items.Count)
    {
        selectionIndicator.gameObject.SetActive(false);
        equippedItemText.text = "No item selected";
    }
}

    private void EquipItem(int index)
    {
    if (index >= 0 && index < playerInventory.items.Count)
    {
        var item = playerInventory.items[index];

        // Verificar si el ítem ya está equipado
        if (currentlyEquippedItem == item.name)
        {
            // Desequipar el ítem
            currentlyEquippedItem = "";
            equippedItemText.text = "No item selected";
            equippedItemDisplayText.text = "NoItem";
            selectedIndex = 0; // Regresar al Index 0
            MoveSelectionIndicator(selectedIndex); // Actualizar el indicador de selección
            Debug.Log("Unequipped item: " + item.name);
        }
        else
        {
            // Equipar el nuevo ítem
            currentlyEquippedItem = item.name;
            equippedItemText.text = "Equipped: " + item.name;
            equippedItemDisplayText.text = item.name;
            selectedIndex = index;
            MoveSelectionIndicator(selectedIndex);
            Debug.Log("Equipped item: " + item.name);
        }
    }
    else
    {
        equippedItemText.text = "No item selected";
        equippedItemDisplayText.text = "Noitem";
    }
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
        selectionIndicator.gameObject.SetActive(false); // Oculta el indicador si el índice no es válido o el botón está desactivado
    }
}

       private void UseEquippedItem()
{
    if (playerInventory.items.Count > 0 && selectedIndex >= 0 && selectedIndex < playerInventory.items.Count)
    {
        var equippedItem = playerInventory.items[selectedIndex];

        // Si el Index 0 está seleccionado, mostrar "Sin Item equipado"
        if (selectedIndex == 0)
        {
            Debug.Log("No Equipped item");
            equippedItemText.text = "No Equipped item";
            return;
        }

        if (equippedItem.quantity > 0 || equippedItem.quantity > infiniteThreshold)
        {
            Debug.Log("Used: " + equippedItem.name);

            if (equippedItem.quantity <= infiniteThreshold)
            {
                equippedItem.quantity--; // Modifica directamente el ScriptableObject

                if (equippedItem.quantity == 0)
                {
                    // Regresar al índice 0 cuando el ítem se agota
                    selectedIndex = 0;
                    EquipItem(selectedIndex); // Equipar el ítem en el índice 0
                    MoveSelectionIndicator(selectedIndex); // Mover el indicador de selección
                }
            }
            else
            {
                Debug.Log("Infinite Item used: " + equippedItem.name);
            }

            UpdateInventoryUI(); // Actualiza la UI después de usar el ítem
        }
        else if (equippedItem.name == "NoItem")
        {
            Debug.Log("No Item Selected, can't do anything");
        }
    }
}
}