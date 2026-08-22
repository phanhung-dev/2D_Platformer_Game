using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        if (countText == null) return;

        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (item == null) return;

        if (item.type == ItemType.Consumable)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Damageable damageable = player.GetComponent<Damageable>();
                Mana playerMana = player.GetComponent<Mana>();
                if (damageable != null && damageable.IsAlive)
                {
                    bool itemUsed = false;

                    if (item.maxHealthBonus > 0)
                    {
                        damageable.IncreaseMaxHealth(item.maxHealthBonus);
                        itemUsed = true;
                    }

                    else if (item.healAmount > 0 && damageable.Health < damageable.MaxHealth)
                    {
                        damageable.Heal(item.healAmount);
                        itemUsed = true;
                    }
                    else if (item.maxManaBonus > 0 && playerMana != null)
                    {
                        playerMana.IncreaseMaxMana(item.maxManaBonus);
                        itemUsed = true;
                    }
                    else if (item.manaRestoreAmount > 0 && playerMana != null && playerMana.CurrentMana < playerMana.MaxMana)
                    {
                        playerMana.RestoreMana(item.manaRestoreAmount);
                        itemUsed = true;
                    }

                    if (itemUsed)
                    {
                        count--;
                        if (count <= 0)
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            RefreshCount();
                        }
                    }
                }
            }
           
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true; 
        transform.SetParent(parentAfterDrag);
    }
}
