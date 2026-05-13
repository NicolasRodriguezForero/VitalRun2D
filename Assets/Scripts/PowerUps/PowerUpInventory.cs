using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupInventory : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int maxSlots = 3;

    [Header("Parámetros de potenciadores")]
    [SerializeField] private float relojSegundos = 5f;
    [SerializeField] private float carritoDuracion = 15f;
    [SerializeField] private int carritoMaxItems = 2;

    private List<PowerupType> slots = new List<PowerupType>();
    private bool carritoActivo = false;

    public int SlotCount => slots.Count;
    public bool IsCarritoActive => carritoActivo;
    public int CarritoMaxItems => carritoMaxItems;

    public bool AddPowerup(PowerupType type)
    {
        if (slots.Count >= maxSlots)
        {
            Debug.Log("Inventario de potenciadores lleno");
            return false;
        }

        slots.Add(type);
        Debug.Log("Potenciador recogido: " + type + " (" + slots.Count + "/" + maxSlots + ")");
        return true;
    }

    public void UseSlot(int index)
    {
        if (index < 0 || index >= slots.Count) return;

        PowerupType type = slots[index];
        slots.RemoveAt(index);

        ActivatePowerup(type);
    }

    private void ActivatePowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Reloj:
                GameTimer.Instance.AddTime(relojSegundos);
                Debug.Log("Reloj usado: +" + relojSegundos + " seg");
                break;

            case PowerupType.Carrito:
                StartCoroutine(CarritoCoroutine());
                break;

            case PowerupType.DoblePuntuacion:
                ScoreManager.Instance.doublePointsActive = true;
                Debug.Log("Doble puntuacion activada para la siguiente entrega");
                break;
        }
    }

    private IEnumerator CarritoCoroutine()
    {
        carritoActivo = true;
        Debug.Log("Carrito activo por " + carritoDuracion + " seg");
        yield return new WaitForSeconds(carritoDuracion);
        carritoActivo = false;
        Debug.Log("Carrito desactivado");
    }

    // Para conectar al HUD luego
    public PowerupType? GetSlot(int index)
    {
        if (index < 0 || index >= slots.Count) return null;
        return slots[index];
    }
}