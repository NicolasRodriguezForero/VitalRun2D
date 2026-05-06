using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "VitalRun/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public enum Category { Medical, Food, Water, Logistics }
    public Category category;
}