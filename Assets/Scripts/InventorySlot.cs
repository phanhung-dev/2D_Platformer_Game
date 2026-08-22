using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image slotImage;
    public Color selectedColor = Color.yellow;
    public Color notSelectedColor = Color.white;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        if (slotImage != null) slotImage.color = selectedColor;
    }

    public void Deselect()
    {
        if (slotImage != null)
        {
            slotImage.color = notSelectedColor;
        }
    }
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
