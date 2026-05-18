using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton que bloquea habitaciones aleatoriamente.
/// Cada 25 seg bloquea una habitación libre durante 10 seg.
/// Nunca hay más de una bloqueada a la vez.
/// </summary>
public class RoomBlocker : MonoBehaviour
{
    public static RoomBlocker Instance { get; private set; }

    [Header("Habitaciones bloqueables")]
    [Tooltip("Lista de las 3 habitaciones bloqueables (Norte, Este, Oeste)")]
    public List<Room> rooms = new List<Room>();

    [Header("Configuración de tiempo")]
    [Tooltip("Tiempo entre bloqueos (segundos)")]
    public float intervaloEntreBloqueos = 25f;

    [Tooltip("Duración de cada bloqueo (segundos)")]
    public float duracionBloqueo = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogWarning("[RoomBlocker] No hay habitaciones asignadas.");
            return;
        }

        StartCoroutine(CicloDeBloqueo());
    }

    private IEnumerator CicloDeBloqueo()
    {
        // Espera inicial antes del primer bloqueo
        yield return new WaitForSeconds(intervaloEntreBloqueos);

        while (true)
        {
            Room habitacionElegida = ElegirHabitacionLibre();

            if (habitacionElegida != null)
            {
                habitacionElegida.Block();
                Debug.Log($"[RoomBlocker] Bloqueada: {habitacionElegida.roomName}");

                yield return new WaitForSeconds(duracionBloqueo);

                habitacionElegida.Unblock();
                Debug.Log($"[RoomBlocker] Desbloqueada: {habitacionElegida.roomName}");
            }

            yield return new WaitForSeconds(intervaloEntreBloqueos);
        }
    }

    /// <summary>
    /// Elige aleatoriamente una habitación que no esté bloqueada.
    /// </summary>
    private Room ElegirHabitacionLibre()
    {
        List<Room> libres = new List<Room>();
        foreach (Room r in rooms)
        {
            if (r != null && !r.IsBlocked)
            {
                libres.Add(r);
            }
        }

        if (libres.Count == 0) return null;

        return libres[Random.Range(0, libres.Count)];
    }
}