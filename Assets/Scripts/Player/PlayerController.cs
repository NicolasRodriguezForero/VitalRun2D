using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Este método lo conectas desde el componente Player Input
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
{
    if (!context.performed) return;

    Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 1.5f);

    // Primero intentar depositar en mesa (si tiene item)
    PlayerInventory inv = GetComponent<PlayerInventory>();
    if (inv != null && inv.GetCurrentItem() != null)
    {
        foreach (Collider2D col in nearby)
        {
            PackingTable table = col.GetComponent<PackingTable>();
            if (table != null)
            {
                table.TryDeposit();
                return;
            }
        }
    }

    // Si no hay mesa o no tiene item, intentar recoger
    foreach (Collider2D col in nearby)
    {
        ItemPickup item = col.GetComponent<ItemPickup>();
        if (item != null)
        {
            item.TryPickUp();
            return;
        }
    }
}

    public void OnDrop(InputAction.CallbackContext context)
{
    if (!context.performed) return;

    PlayerInventory inv = GetComponent<PlayerInventory>();
    if (inv != null) inv.RemoveItem();
    Debug.Log("Dropped item");
}

    void FixedUpdate()
    {
        Vector2 movement = moveInput.normalized;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // Actualizar Animator si existe
        if (animator != null)
        {
            animator.SetBool("isMoving", movement.magnitude > 0.1f);
            if (movement.magnitude > 0.1f)
            {
                animator.SetFloat("MoveX", movement.x);
                animator.SetFloat("MoveY", movement.y);
            }
        }
    }
}