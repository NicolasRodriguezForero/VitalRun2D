using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrdersUI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform ordersPanel;        // El OrdersPanel del Canvas
    public GameObject orderSlotPrefab;   // Prefab del OrderSlot

    [Header("Colores de cajas")]
    public Color grayColor = Color.gray;
    public Color greenColor = Color.green;
    public Color yellowColor = Color.yellow;
    public Color blueColor = Color.blue;

    private List<GameObject> activeSlots = new List<GameObject>();

    void Update()
    {
        RefreshOrdersUI();
    }

    void RefreshOrdersUI()
    {
        if (OrderManager.Instance == null) return;

        List<OrderData> ordenes = OrderManager.Instance.GetActiveOrders();

        // Crear slots faltantes
        while (activeSlots.Count < ordenes.Count)
        {
            GameObject newSlot = Instantiate(orderSlotPrefab, ordersPanel);
            activeSlots.Add(newSlot);
        }

        // Configurar slots activos y ocultar los sobrantes
        for (int i = 0; i < activeSlots.Count; i++)
        {
            if (i < ordenes.Count)
            {
                activeSlots[i].SetActive(true);
                ConfigurarSlot(activeSlots[i], ordenes[i]);
            }
            else
            {
                activeSlots[i].SetActive(false);
            }
        }
    }

    void ConfigurarSlot(GameObject slot, OrderData orden)
    {
        // Color de la caja
        Image boxImage = slot.transform.Find("BoxImage").GetComponent<Image>();
        boxImage.color = GetColorFromEnum(orden.boxColor);

        // Destino
        TMP_Text destText = slot.transform.Find("DestinationText").GetComponent<TMP_Text>();
        destText.text = orden.destination.ToUpper();

        // Puntos
        TMP_Text ptsText = slot.transform.Find("PointsText").GetComponent<TMP_Text>();
        ptsText.text = "+" + orden.basePoints + " pts";

        // Items requeridos (íconos)
        Transform itemsContainer = slot.transform.Find("ItemsContainer");
        for (int j = 0; j < itemsContainer.childCount; j++)
        {
            Image itemIcon = itemsContainer.GetChild(j).GetComponent<Image>();
            if (j < orden.requiredItems.Count && orden.requiredItems[j] != null)
            {
                itemIcon.sprite = orden.requiredItems[j].itemSprite;
                itemIcon.color = Color.white; // resetear color para que se vea el sprite
                itemIcon.enabled = true;
            }
            else
            {
                itemIcon.enabled = false;
            }
        }
    }

    Color GetColorFromEnum(OrderData.BoxColor boxColor)
    {
        switch (boxColor)
        {
            case OrderData.BoxColor.Gray: return grayColor;
            case OrderData.BoxColor.Green: return greenColor;
            case OrderData.BoxColor.Yellow: return yellowColor;
            case OrderData.BoxColor.Blue: return blueColor;
            default: return Color.white;
        }
    }
}