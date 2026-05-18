using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("Configuración")]
    public List<OrderData> availableOrders = new List<OrderData>();
    public int maxActiveOrders = 3;
    private List<OrderData> activeOrders = new List<OrderData>();
    public int CompletedOrders { get; private set; } = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Generar las primeras 3 órdenes
        for (int i = 0; i < maxActiveOrders; i++)
        {
            GenerateNewOrder();
        }
    }

    void GenerateNewOrder()
    {
        if (availableOrders.Count == 0)
        {
            Debug.LogWarning("No hay órdenes disponibles para generar");
            return;
        }

        OrderData newOrder = availableOrders[Random.Range(0, availableOrders.Count)];
        activeOrders.Add(newOrder);
        Debug.Log("Nueva orden: " + newOrder.destination + " (" + newOrder.boxColor + ") - " + newOrder.basePoints + " pts");
    }

    public List<OrderData> GetActiveOrders()
    {
        return activeOrders;
    }

    public void CompleteOrder(OrderData order)
    {
        if (activeOrders.Contains(order))
        {
            activeOrders.Remove(order);
            CompletedOrders++;
            Debug.Log("Orden completada: " + order.destination + " (total: " + CompletedOrders + ")");
            GenerateNewOrder();
        }
    }
}   