using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public float respawnTime = 8f;

    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    private bool playerNearby = false;
    private PlayerInventory playerInventory;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();

        // Si tiene ItemData asignado, usar su sprite automáticamente
        if (itemData != null && itemData.itemSprite != null)
        {
            spriteRenderer.sprite = itemData.itemSprite;
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

    // Lo llama el PlayerController cuando se presiona Interact
    public void TryPickUp()
    {
        Debug.Log("TryPickUp llamado. playerNearby=" + playerNearby + " inventory=" + (playerInventory != null));
        if (!playerNearby || playerInventory == null) return;

        if (playerInventory.CanPickUp())
        {
            playerInventory.AddItem(itemData);
            Hide();
            Invoke(nameof(Respawn), respawnTime);
        }
        else
        {
            Debug.Log("Inventario lleno");
        }
    }

    void Hide()
    {
        spriteRenderer.enabled = false;
        itemCollider.enabled = false;
    }

    void Respawn()
    {
        spriteRenderer.enabled = true;
        itemCollider.enabled = true;

        Debug.Log("Item reaparecio. Verificando jugador cercano...");

        Collider2D[] overlapping = Physics2D.OverlapBoxAll(
            transform.position,
            itemCollider.bounds.size,
            0f
        );

        Debug.Log("Colliders encontrados: " + overlapping.Length);

        foreach (Collider2D col in overlapping)
        {
            Debug.Log("Detectado: " + col.name + " con tag: " + col.tag);
            if (col.CompareTag("Player"))
            {
                playerNearby = true;
                playerInventory = col.GetComponent<PlayerInventory>();
                Debug.Log("Jugador detectado en respawn!");
                break;
            }
        }
    }
}