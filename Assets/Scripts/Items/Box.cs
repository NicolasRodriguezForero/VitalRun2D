using UnityEngine;

public class Box : MonoBehaviour
{
    public OrderData order;

    private SpriteRenderer spriteRenderer;
    private Collider2D boxCollider;
    private bool playerNearby = false;
    private PlayerInventory playerInventory;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();
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

    public void TryPickUp()
    {
        if (!playerNearby || playerInventory == null) return;

        if (playerInventory.CanPickUp())
        {
            playerInventory.AddBox(this);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Inventario lleno");
        }
    }
}