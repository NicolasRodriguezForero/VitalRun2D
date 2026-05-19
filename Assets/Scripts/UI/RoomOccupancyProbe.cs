using UnityEngine;

/// <summary>
/// Coloca este script en un GameObject hijo de Room con un Collider2D (isTrigger = true)
/// que cubra el interior de la habitación. Notifica a Room cuando el jugador entra o sale.
/// </summary>
public class RoomOccupancyProbe : MonoBehaviour
{
    [SerializeField] private Room room;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) room.SetPlayerInside(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) room.SetPlayerInside(false);
    }
}
