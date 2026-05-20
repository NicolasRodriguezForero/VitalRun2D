using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrdersUI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform ordersPanel;        // El OrdersPanel del Canvas
    public GameObject orderSlotPrefab;   // Prefab del OrderSlot

    [Header("Sprite general para todas las cajas (se tinta por color)")]
    public Sprite boxSprite;

    [Header("Tintes por color (mismos que Box.cs)")]
    public Color grayColor = new Color(0.75f, 0.75f, 0.78f);
    public Color greenColor = new Color(0.35f, 1f, 0.45f);
    public Color yellowColor = new Color(1f, 0.95f, 0.2f);
    public Color blueColor = new Color(0.35f, 0.75f, 1f);

    [Header("Layout del slot")]
    public float padding = 8f;
    public float boxSize = 64f;
    public float itemSize = 56f;
    public float itemSpacing = 4f;
    public float gapBoxItems = 6f;
    public int destinationFontSize = 26;
    public int pointsFontSize = 20;

    [Header("Estilo de la tarjeta")]
    public Color cardFillColor = new Color(0.96f, 0.91f, 0.78f, 1f);   // beige cálido
    public Color cardBorderColor = new Color(0.55f, 0.40f, 0.22f, 1f); // marrón borde
    public Vector2 cardBorderDistance = new Vector2(2f, -2f);
    public Color cardShadowColor = new Color(0f, 0f, 0f, 0.35f);
    public Vector2 cardShadowDistance = new Vector2(3f, -3f);

    private List<GameObject> activeSlots = new List<GameObject>();
    private HashSet<GameObject> laidOut = new HashSet<GameObject>();

    void Update()
    {
        RefreshOrdersUI();
    }

    void RefreshOrdersUI()
    {
        if (OrderManager.Instance == null) return;

        List<OrderData> ordenes = OrderManager.Instance.GetActiveOrders();

        while (activeSlots.Count < ordenes.Count)
        {
            GameObject newSlot = Instantiate(orderSlotPrefab, ordersPanel);
            activeSlots.Add(newSlot);
        }

        for (int i = 0; i < activeSlots.Count; i++)
        {
            if (i < ordenes.Count)
            {
                activeSlots[i].SetActive(true);
                if (!laidOut.Contains(activeSlots[i]))
                {
                    AplicarLayout(activeSlots[i]);
                    laidOut.Add(activeSlots[i]);
                }
                ConfigurarSlot(activeSlots[i], ordenes[i]);
            }
            else
            {
                activeSlots[i].SetActive(false);
            }
        }
    }

    void AplicarLayout(GameObject slot)
    {
        RectTransform box = slot.transform.Find("BoxImage") as RectTransform;
        RectTransform items = slot.transform.Find("ItemsContainer") as RectTransform;
        RectTransform dest = slot.transform.Find("DestinationText") as RectTransform;
        RectTransform pts = slot.transform.Find("PointsText") as RectTransform;

        // ---- Fondo beige + borde + sombrita en cada tarjeta ----
        {
            Image bg = slot.GetComponent<Image>();
            if (bg == null) bg = slot.AddComponent<Image>();
            bg.sprite = null;                  // rectángulo sólido (sprite UI default)
            bg.color = cardFillColor;
            bg.type = Image.Type.Simple;
            bg.raycastTarget = false;

            Outline outline = slot.GetComponent<Outline>();
            if (outline == null) outline = slot.AddComponent<Outline>();
            outline.effectColor = cardBorderColor;
            outline.effectDistance = cardBorderDistance;
            outline.useGraphicAlpha = true;

            Shadow shadow = slot.GetComponent<Shadow>();
            // Outline hereda de Shadow, así que pedimos específicamente el Shadow puro
            Shadow[] allShadows = slot.GetComponents<Shadow>();
            bool hasPureShadow = false;
            foreach (Shadow s in allShadows)
            {
                if (s.GetType() == typeof(Shadow))
                {
                    s.effectColor = cardShadowColor;
                    s.effectDistance = cardShadowDistance;
                    s.useGraphicAlpha = true;
                    hasPureShadow = true;
                    break;
                }
            }
            if (!hasPureShadow)
            {
                Shadow s = slot.AddComponent<Shadow>();
                s.effectColor = cardShadowColor;
                s.effectDistance = cardShadowDistance;
                s.useGraphicAlpha = true;
            }
        }

        // ---- BoxImage: anclado al CENTRO-IZQUIERDA del slot ----
        if (box != null)
        {
            box.anchorMin = new Vector2(0f, 0.5f);
            box.anchorMax = new Vector2(0f, 0.5f);
            box.pivot = new Vector2(0f, 0.5f);
            box.sizeDelta = new Vector2(boxSize, boxSize);
            box.anchoredPosition = new Vector2(padding, 0f);
        }

        // ---- ItemsContainer: a la derecha de la caja, también anclado a la izquierda ----
        if (items != null)
        {
            items.anchorMin = new Vector2(0f, 0.5f);
            items.anchorMax = new Vector2(0f, 0.5f);
            items.pivot = new Vector2(0f, 0.5f);

            int slots = items.childCount;
            float width = slots * itemSize + Mathf.Max(0, slots - 1) * itemSpacing;
            items.sizeDelta = new Vector2(width, itemSize);
            items.anchoredPosition = new Vector2(padding + boxSize + gapBoxItems, 0f);

            HorizontalLayoutGroup hlg = items.GetComponent<HorizontalLayoutGroup>();
            if (hlg == null) hlg = items.gameObject.AddComponent<HorizontalLayoutGroup>();
            hlg.childAlignment = TextAnchor.MiddleLeft;
            hlg.spacing = itemSpacing;
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = false;
            hlg.childControlWidth = false;
            hlg.childControlHeight = false;
            hlg.padding = new RectOffset(0, 0, 0, 0);

            for (int j = 0; j < items.childCount; j++)
            {
                RectTransform child = items.GetChild(j) as RectTransform;
                if (child == null) continue;
                child.sizeDelta = new Vector2(itemSize, itemSize);
                child.localScale = Vector3.one;
            }
        }

        // ---- DestinationText: arriba-derecha ----
        if (dest != null)
        {
            dest.anchorMin = new Vector2(1f, 1f);
            dest.anchorMax = new Vector2(1f, 1f);
            dest.pivot = new Vector2(1f, 1f);
            dest.anchoredPosition = new Vector2(-padding, -padding);
            dest.sizeDelta = new Vector2(160f, 28f);

            TMP_Text t = dest.GetComponent<TMP_Text>();
            if (t != null)
            {
                t.alignment = TextAlignmentOptions.TopRight;
                t.enableAutoSizing = true;
                t.fontSizeMin = 10;
                t.fontSizeMax = destinationFontSize;
                t.enableWordWrapping = false;
                t.overflowMode = TextOverflowModes.Ellipsis;
            }
        }

        // ---- PointsText: abajo-derecha ----
        if (pts != null)
        {
            pts.anchorMin = new Vector2(1f, 0f);
            pts.anchorMax = new Vector2(1f, 0f);
            pts.pivot = new Vector2(1f, 0f);
            pts.anchoredPosition = new Vector2(-padding, padding);
            pts.sizeDelta = new Vector2(100f, 22f);

            TMP_Text t = pts.GetComponent<TMP_Text>();
            if (t != null)
            {
                t.alignment = TextAlignmentOptions.BottomRight;
                t.fontSize = pointsFontSize;
                t.enableWordWrapping = false;
            }
        }
    }

    void ConfigurarSlot(GameObject slot, OrderData orden)
    {
        Image boxImage = slot.transform.Find("BoxImage").GetComponent<Image>();
        boxImage.sprite = boxSprite;                       // mismo sprite para todas
        boxImage.color = GetColorFromEnum(orden.boxColor); // se le aplica el tinte por color

        TMP_Text destText = slot.transform.Find("DestinationText").GetComponent<TMP_Text>();
        destText.text = orden.destination.ToUpper();

        TMP_Text ptsText = slot.transform.Find("PointsText").GetComponent<TMP_Text>();
        ptsText.text = "+" + orden.basePoints + " pts";

        Transform itemsContainer = slot.transform.Find("ItemsContainer");
        for (int j = 0; j < itemsContainer.childCount; j++)
        {
            Image itemIcon = itemsContainer.GetChild(j).GetComponent<Image>();
            if (j < orden.requiredItems.Count && orden.requiredItems[j] != null)
            {
                itemIcon.sprite = orden.requiredItems[j].itemSprite;
                itemIcon.color = Color.white;
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
