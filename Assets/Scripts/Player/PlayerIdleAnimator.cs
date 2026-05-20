using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerIdleAnimator : MonoBehaviour
{
    public enum Facing { Down, Up, Right, Left }

    [Header("Sprites idle por dirección (5 frames cada uno)")]
    public Sprite[] idleUp;
    public Sprite[] idleDown;
    public Sprite[] idleRight;

    [Header("Configuración")]
    public float frameDuration = 0.18f;
    public Facing initialFacing = Facing.Down;
    [Tooltip("Umbral mínimo de velocidad para considerar que el jugador se está moviendo")]
    public float movementThreshold = 0.02f;

    [Header("Escala visual por dirección (compensa diferencias de tamaño entre filas del sheet)")]
    public float scaleUp = 1.0f;
    public float scaleDown = 1.08f;
    public float scaleRight = 1.0f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Facing facing;
    private int frameIndex;
    private float frameTimer;
    private Vector2 lastPos;
    private Vector3 baseScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        facing = initialFacing;
        lastPos = rb != null ? rb.position : (Vector2)transform.position;
        baseScale = transform.localScale;
        ApplySprite();
    }

    void Update()
    {
        Vector2 currentPos = rb != null ? rb.position : (Vector2)transform.position;
        Vector2 delta = currentPos - lastPos;
        lastPos = currentPos;

        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 1e-5f);

        if (speed > movementThreshold)
        {
            facing = FacingFromDelta(delta);
            frameIndex = 0;
            frameTimer = 0f;
            ApplySprite();
            return;
        }

        frameTimer += Time.deltaTime;
        if (frameTimer >= frameDuration)
        {
            frameTimer -= frameDuration;
            Sprite[] arr = GetSpritesFor(facing);
            if (arr != null && arr.Length > 0)
            {
                frameIndex = (frameIndex + 1) % arr.Length;
            }
            ApplySprite();
        }
    }

    Facing FacingFromDelta(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
        {
            return delta.x >= 0f ? Facing.Right : Facing.Left;
        }
        return delta.y >= 0f ? Facing.Up : Facing.Down;
    }

    Sprite[] GetSpritesFor(Facing f)
    {
        switch (f)
        {
            case Facing.Up: return idleUp;
            case Facing.Down: return idleDown;
            case Facing.Right:
            case Facing.Left:
            default: return idleRight;
        }
    }

    void ApplySprite()
    {
        if (sr == null) return;
        Sprite[] arr = GetSpritesFor(facing);
        if (arr == null || arr.Length == 0) return;
        int i = Mathf.Clamp(frameIndex, 0, arr.Length - 1);
        sr.sprite = arr[i];
        sr.flipX = (facing == Facing.Left);

        float s = GetScaleFor(facing);
        transform.localScale = new Vector3(baseScale.x * s, baseScale.y * s, baseScale.z);
    }

    float GetScaleFor(Facing f)
    {
        switch (f)
        {
            case Facing.Up: return scaleUp;
            case Facing.Down: return scaleDown;
            case Facing.Right:
            case Facing.Left:
            default: return scaleRight;
        }
    }

    public void SetFacing(Facing f)
    {
        facing = f;
        frameIndex = 0;
        frameTimer = 0f;
        ApplySprite();
    }
}
