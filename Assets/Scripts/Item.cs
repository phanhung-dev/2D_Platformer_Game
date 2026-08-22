using UnityEngine;

public enum ItemType
{
    Consumable,
    Tool,
    Weapon,
}

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Information")]
    public string itemName;
    public Sprite image;
    public ItemType type = ItemType.Consumable;

    public bool stackable = true;

    [Header("Health")]
    public int healAmount = 25;

    public int maxHealthBonus = 0;

    [Header("Mana")]
    public int manaRestoreAmount = 0;
    public int maxManaBonus = 0;
}
