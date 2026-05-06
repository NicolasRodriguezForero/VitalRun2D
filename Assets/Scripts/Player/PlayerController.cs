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