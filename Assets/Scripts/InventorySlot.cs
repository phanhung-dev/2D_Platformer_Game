using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (inventoryItem != null )
            {
                inventoryItem.parentAfterDrag = transform;
            }
        }
    }

    public bool IsEmpty()
    {
        return transform.childCount == 0;
    }
}
