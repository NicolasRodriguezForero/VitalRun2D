using UnityEngine;

public class DispatchBox : MonoBehaviour
{
    public OrderData.BoxColor acceptedColor;

    private bool playerNearby = false;
    private PlayerInventory playerInventory;

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

    public void TryDeliver()
    {
        if (!playerNearby || playerInventory == null) return;

        Box box = playerInventory.GetCurrentBox();
        if (box == null) return;

        // Verificar si el color de la caja coincide con el del buzón
        if (box.order.boxColor == acceptedColor)
        {
            Debug.Log("¡Entrega correcta! " + box.order.destination + " - " + box.order.basePoints + " pts");

            // Notificar al ScoreManager (lo crearemos en el siguiente paso)
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(box.order);
            }

            // Limpiar caja del inventario y destruirla
            playerInventory.RemoveBox();
            Destroy(box.gameObject);
        }
        else
        {
            Debug.Log("Buzón equivocado. Esta caja es " + box.order.boxColor + ", el buzón acepta " + acceptedColor);
        }
    }
}