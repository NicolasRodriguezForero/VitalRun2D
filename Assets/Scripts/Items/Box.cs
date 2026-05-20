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
                spriteRenderer.color = new Color(0.75f, 0.75f, 0.78f);
                break;
            case OrderData.BoxColor.Green:
                spriteRenderer.color = new Color(0.35f, 1f, 0.45f);
                break;
            case OrderData.BoxColor.Yellow:
                spriteRenderer.color = new Color(1f, 0.95f, 0.2f);
                break;
            case OrderData.BoxColor.Blue:
                spriteRenderer.color = new Color(0.35f, 0.75f, 1f);
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