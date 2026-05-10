using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxItems = 1;
    private ItemData currentItem;
    private Box currentBox;

    public bool CanPickUp()
    {
        return currentItem == null && currentBox == null;
    }

    public void AddItem(ItemData item)
    {
        currentItem = item;
        Debug.Log("Recogiste: " + item.itemName);
    }

    public void AddBox(Box box)
    {
        currentBox = box;
        Debug.Log("Cargas una caja " + box.order.boxColor + " para " + box.order.destination);
    }

    public ItemData GetCurrentItem()
    {
        return currentItem;
    }

    public Box GetCurrentBox()
    {
        return currentBox;
    }

    public void RemoveItem()
    {
        if (currentItem != null)
        {
            Debug.Log("Soltaste: " + currentItem.itemName);
            currentItem = null;
        }
    }

    public void RemoveBox()
    {
        if (currentBox != null)
        {
            Debug.Log("Soltaste la caja");
            currentBox = null;
        }
    }
}