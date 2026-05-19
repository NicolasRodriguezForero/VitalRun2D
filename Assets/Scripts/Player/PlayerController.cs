using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    // --- Sistema de boost de velocidad ---
    private float originalSpeed;
    private Coroutine speedBoostCoroutine;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 rawInput;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalSpeed = speed; // <-- AGREGAR esta línea
    }
    // Este método lo conectas desde el componente Player Input
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        bool xJustPressed = input.x != 0 && rawInput.x == 0;
        bool yJustPressed = input.y != 0 && rawInput.y == 0;
        rawInput = input;

        if (input == Vector2.zero)
            moveInput = Vector2.zero;
        else if (xJustPressed)
            moveInput = new Vector2(input.x, 0);
        else if (yJustPressed)
            moveInput = new Vector2(0, input.y);
        else if (input.x == 0)
            moveInput = new Vector2(0, input.y);
        else if (input.y == 0)
            moveInput = new Vector2(input.x, 0);
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

    /// <summary>
    /// Aplica un boost de velocidad temporal. Si ya hay un boost activo, lo cancela y aplica el nuevo.
    /// </summary>
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
            speed = originalSpeed; // restaurar antes de aplicar el nuevo
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private System.Collections.IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        speed = originalSpeed * multiplier;
        Debug.Log("Velocidad x" + multiplier + " activada por " + duration + " seg");
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        speedBoostCoroutine = null;
        Debug.Log("Velocidad restaurada a " + originalSpeed);
    }
}