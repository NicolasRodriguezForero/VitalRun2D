using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackingTable : MonoBehaviour
{
    [Header("Configuración temporal (hasta Fase 6)")]
    public List<ItemData> requiredItems = new List<ItemData>();
    public GameObject boxPrefab;

    [Header("UI")]
    public Slider progressBar;

    [Header("Timing")]
    public float packingTime = 10f;

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

    // Lo llama el PlayerController cuando se presiona Interact
    public void TryDeposit()
    {
        if (!playerNearby || playerInventory == null || isPacking) return;

        ItemData currentItem = playerInventory.GetCurrentItem();
        if (currentItem == null)
        {
            Debug.Log("No tienes item para depositar");
            return;
        }

        // Verificar si este item es necesario
        if (requiredItems.Contains(currentItem) && !depositedItems.Contains(currentItem))
        {
            depositedItems.Add(currentItem);
            playerInventory.RemoveItem();
            Debug.Log("Depositado: " + currentItem.itemName + " (" + depositedItems.Count + "/" + requiredItems.Count + ")");

            // Si ya están todos: empezar a empaquetar
            if (depositedItems.Count == requiredItems.Count)
            {
                StartCoroutine(PackBox());
            }
        }
        else
        {
            Debug.Log("Este item no es necesario o ya fue depositado");
        }
    }

    IEnumerator PackBox()
    {
        isPacking = true;
        Debug.Log("Empaquetando...");

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

        // Crear la caja
        if (boxPrefab != null)
        {
            Vector3 boxPos = transform.position + new Vector3(0, 0, 0);
            Instantiate(boxPrefab, boxPos, Quaternion.identity);
            Debug.Log("Caja lista!");
        }

        // Resetear mesa
        depositedItems.Clear();
        if (progressBar != null) progressBar.gameObject.SetActive(false);
        isPacking = false;
    }
}