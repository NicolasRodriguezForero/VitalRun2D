using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxItems = 1;
    private ItemData currentItem;

    public bool CanPickUp()
    {
        return currentItem == null;
    }

    public void AddItem(ItemData item)
    {
        currentItem = item;
        Debug.Log("Recogiste: " + item.itemName);
    }

    public ItemData GetCurrentItem()
    {
        return currentItem;
    }

    public void RemoveItem()
    {
        if (currentItem != null)
        {
            Debug.Log("Soltaste: " + currentItem.itemName);
            currentItem = null;
        }
    }
}