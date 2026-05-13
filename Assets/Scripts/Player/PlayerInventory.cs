using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemData> currentItems = new List<ItemData>();
    private Box currentBox;
    private PowerupInventory powerupInventory;

    void Start()
    {
        powerupInventory = GetComponent<PowerupInventory>();
    }

    private int GetMaxItems()
    {
        if (powerupInventory != null && powerupInventory.IsCarritoActive)
            return powerupInventory.CarritoMaxItems;
        return 1;
    }

    public bool CanPickUp()
    {
        // No se puede recoger si lleva caja o si ya tiene el maximo de items
        if (currentBox != null) return false;
        return currentItems.Count < GetMaxItems();
    }

    public void AddItem(ItemData item)
    {
        currentItems.Add(item);
        Debug.Log("Recogiste: " + item.itemName + " (" + currentItems.Count + "/" + GetMaxItems() + ")");
    }

    public void AddBox(Box box)
    {
        currentBox = box;
        Debug.Log("Cargas una caja " + box.order.boxColor + " para " + box.order.destination);
    }

    // Devuelve el primer item (compatibilidad con codigo existente)
    public ItemData GetCurrentItem()
    {
        if (currentItems.Count == 0) return null;
        return currentItems[0];
    }

    public List<ItemData> GetAllItems()
    {
        return currentItems;
    }

    public Box GetCurrentBox()
    {
        return currentBox;
    }

    public void RemoveItem()
    {
        if (currentItems.Count > 0)
        {
            Debug.Log("Soltaste: " + currentItems[0].itemName);
            currentItems.RemoveAt(0);
        }
    }

    public void RemoveAllItems()
    {
        currentItems.Clear();
    }

    public void RemoveBox()
    {
        if (currentBox != null)
        {
            Debug.Log("Soltaste la caja");
            currentBox = null;
        }
    }

    public bool HasItems()
    {
        return currentItems.Count > 0;
    }
}