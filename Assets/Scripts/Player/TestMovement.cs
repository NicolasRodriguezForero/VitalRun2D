using UnityEngine;
public class TestMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start() { rb = GetComponent<Rigidbody2D>(); }
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + new Vector2(x, y).normalized * 5f * Time.fixedDeltaTime);
    }
}