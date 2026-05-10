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
        Vector2 input = context.ReadValue<Vector2>();

        // Solo permitir un eje a la vez (priorizar el de mayor magnitud)
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            moveInput = new Vector2(input.x, 0);
        }
        else if (Mathf.Abs(input.y) > 0)
        {
            moveInput = new Vector2(0, input.y);
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        PlayerInventory inv = GetComponent<PlayerInventory>();

        // Si tiene item: intentar depositar en mesa
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

        // Si tiene caja: intentar entregar en buzón
        if (inv != null && inv.GetCurrentBox() != null)
        {
            foreach (Collider2D col in nearby)
            {
                DispatchBox dispatch = col.GetComponent<DispatchBox>();
                if (dispatch != null)
                {
                    dispatch.TryDeliver();
                    return;
                }
            }
        }

        // Si no tiene nada: intentar recoger item o caja
        foreach (Collider2D col in nearby)
        {
            ItemPickup item = col.GetComponent<ItemPickup>();
            if (item != null)
            {
                item.TryPickUp();
                return;
            }

            Box box = col.GetComponent<Box>();
            if (box != null)
            {
                box.TryPickUp();
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