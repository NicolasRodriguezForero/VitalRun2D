using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{
    [Header("Referencias")]
    public PowerupInventory inventory;   // El PowerupInventory del Player

    [Header("Botones de slots")]
    public Button[] slotButtons = new Button[3];     // PowerupSlot1, 2, 3
    public Image[] slotIcons = new Image[3];         // El Image de cada botón

    [Header("Íconos por tipo (orden del enum)")]
    public Sprite velocidadSprite;       // PowerupType.Velocidad = 0
    public Sprite carritoSprite;         // PowerupType.Carrito = 1
    public Sprite doblePuntuacionSprite; // PowerupType.DoblePuntuacion = 2

    void Start()
    {
        // Conectar cada botón a su UseSlot correspondiente
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i; // captura local para el closure
            slotButtons[i].onClick.AddListener(() => inventory.UseSlot(index));
        }
    }

    void Update()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        if (inventory == null) return;

        for (int i = 0; i < 3; i++)
        {
            PowerupType? type = inventory.GetSlot(i);

            if (type.HasValue)
            {
                slotIcons[i].sprite = GetSpriteForType(type.Value);
                slotIcons[i].enabled = true;
                slotButtons[i].interactable = true;
            }
            else
            {
                // Slot vacío: ocultar ícono y desactivar botón
                slotIcons[i].enabled = false;
                slotButtons[i].interactable = false;
            }
        }
    }

    Sprite GetSpriteForType(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Velocidad: return velocidadSprite;
            case PowerupType.Carrito: return carritoSprite;
            case PowerupType.DoblePuntuacion: return doblePuntuacionSprite;
            default: return null;
        }
    }
}