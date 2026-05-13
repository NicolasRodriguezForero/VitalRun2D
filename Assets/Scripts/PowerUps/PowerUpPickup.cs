using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private PowerupType type;

    public PowerupType Type => type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerupInventory inventory = other.GetComponent<PowerupInventory>();
            if (inventory != null && inventory.AddPowerup(type))
            {
                Destroy(gameObject);
            }
        }
    }
}