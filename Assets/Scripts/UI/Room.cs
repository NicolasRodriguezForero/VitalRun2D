using UnityEngine;

/// <summary>
/// Representa una habitación lateral que puede ser bloqueada.
/// Asignar a cada GameObject de habitación bloqueable (Norte, Este, Oeste).
/// </summary>
public class Room : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("Nombre legible para debug (Norte, Este, Oeste)")]
    public string roomName;

    [Header("Referencias del bloqueo")]
    [Tooltip("Collider físico (NO trigger) que impide el paso en la entrada")]
    public Collider2D blockerCollider;

    [Tooltip("Overlay oscuro que cubre la habitación cuando está bloqueada")]
    public GameObject darkOverlay;

    [Tooltip("Ícono de candado que aparece cuando está bloqueada")]
    public GameObject lockIcon;

    /// <summary>True si la habitación está actualmente bloqueada.</summary>
    public bool IsBlocked { get; private set; }

    private void Awake()
    {
        // Estado inicial: desbloqueada
        Unblock();
    }

    public void Block()
    {
        IsBlocked = true;
        if (blockerCollider != null) blockerCollider.enabled = true;
        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (lockIcon != null) lockIcon.SetActive(true);
    }

    public void Unblock()
    {
        IsBlocked = false;
        if (blockerCollider != null) blockerCollider.enabled = false;
        if (darkOverlay != null) darkOverlay.SetActive(false);
        if (lockIcon != null) lockIcon.SetActive(false);
    }
}