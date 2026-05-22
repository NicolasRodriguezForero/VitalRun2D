using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackingTable : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject boxPrefab;
    public float packingTime = 10f;

    [Header("UI")]
    public Slider progressBar;

    private OrderData currentOrder;
    private List<ItemData> depositedItems = new List<ItemData>();
    private bool isPacking = false;
    private bool playerNearby = false;
    private PlayerInventory playerInventory;

    void Start()
    {
        if (progressBar != null) progressBar.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            playerInventory = null;
        }
    }

    public void TryDeposit()
    {
        if (!playerNearby || playerInventory == null || isPacking) return;

        ItemData currentItem = playerInventory.GetCurrentItem();
        if (currentItem == null)
        {
            Debug.Log("No tienes item para depositar");
            return;
        }

        // Si la mesa no tiene orden asignada, asignarle una que necesite este item
        if (currentOrder == null)
        {
            currentOrder = FindOrderForItem(currentItem);
            if (currentOrder == null)
            {
                Debug.Log("Ninguna orden activa necesita este item");
                return;
            }
            Debug.Log("Mesa asignada a orden: " + currentOrder.destination);
        }

        // Verificar si el item es necesario para la orden actual
        if (currentOrder.requiredItems.Contains(currentItem) && !depositedItems.Contains(currentItem))
        {
            depositedItems.Add(currentItem);
            playerInventory.RemoveItem();
            ScoreManager.Instance.AddPoints(100);
            Debug.Log("Depositado: " + currentItem.itemName + " (" + depositedItems.Count + "/" + currentOrder.requiredItems.Count + ")");

            if (depositedItems.Count == currentOrder.requiredItems.Count)
            {
                StartCoroutine(PackBox());
            }
        }
        else
        {
            Debug.Log("Este item no sirve para la orden actual");
        }
    }

    OrderData FindOrderForItem(ItemData item)
    {
        if (OrderManager.Instance == null) return null;

        foreach (OrderData order in OrderManager.Instance.GetActiveOrders())
        {
            if (order.requiredItems.Contains(item))
            {
                return order;
            }
        }
        return null;
    }

    IEnumerator PackBox()
    {
        isPacking = true;
        Debug.Log("Empaquetando para " + currentOrder.destination + "...");

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0;
        }

        float elapsed = 0f;
        while (elapsed < packingTime)
        {
            elapsed += Time.deltaTime;
            if (progressBar != null) progressBar.value = elapsed / packingTime;
            yield return null;
        }

        // Crear la caja con el color y orden correspondiente
        if (boxPrefab != null)
        {
            Vector3 boxPos = transform.position + new Vector3(0, 0, 0);
            GameObject newBox = Instantiate(boxPrefab, boxPos, Quaternion.identity);
            Box boxScript = newBox.GetComponent<Box>();
            if (boxScript != null) boxScript.SetOrder(currentOrder);
            Debug.Log("Caja " + currentOrder.boxColor + " lista para " + currentOrder.destination + "!");
        }

        // La orden se marca como completada al ENTREGAR la caja en el buzón, no aquí.
        // Resetear mesa
        currentOrder = null;
        depositedItems.Clear();
        if (progressBar != null) progressBar.gameObject.SetActive(false);
        isPacking = false;
    }
}