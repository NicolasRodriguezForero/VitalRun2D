using UnityEngine;

public class Box : MonoBehaviour
{
    public OrderData order;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetOrder(OrderData newOrder)
    {
        order = newOrder;
        UpdateColor();
    }

    void UpdateColor()
    {
        if (order == null) return;

        switch (order.boxColor)
        {
            case OrderData.BoxColor.Gray:
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case OrderData.BoxColor.Green:
                spriteRenderer.color = new Color(0.4f, 0.8f, 0.4f);
                break;
            case OrderData.BoxColor.Yellow:
                spriteRenderer.color = new Color(1f, 0.9f, 0.3f);
                break;
            case OrderData.BoxColor.Blue:
                spriteRenderer.color = new Color(0.4f, 0.6f, 1f);
                break;
        }
    }
}