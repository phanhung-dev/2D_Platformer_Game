using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int maxStackedItems = 16;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int selectedSlot = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame) ChangeSelectedSlot(0);
            if (Keyboard.current.digit2Key.wasPressedThisFrame) ChangeSelectedSlot(1);
            if (Keyboard.current.digit3Key.wasPressedThisFrame) ChangeSelectedSlot(2);
            if (Keyboard.current.digit4Key.wasPressedThisFrame) ChangeSelectedSlot(3);
        }

        float scroll = Mouse.current != null ? Mouse.current.scroll.ReadValue().y : 0;
        if (scroll > 0f)
        {
            int newSlot = selectedSlot - 1;
            if (newSlot < 0) newSlot = inventorySlots.Length - 1;
            ChangeSelectedSlot(newSlot);
        }
        else if (scroll < 0f)
        {
            int newSlot = selectedSlot + 1;
            if (newSlot >= inventorySlots.Length) newSlot = 0;
            ChangeSelectedSlot(newSlot);
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (newValue < 0 || newValue >= inventorySlots.Length) return;
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null) continue;

            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null) continue;

            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, inventorySlots[i]);
                return true;
            }
        }

        return false;
    }

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
