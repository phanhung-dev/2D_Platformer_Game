using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int maxStackedItems = 16;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
