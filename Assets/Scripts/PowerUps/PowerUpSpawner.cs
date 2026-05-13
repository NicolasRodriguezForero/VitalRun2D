using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float spawnInterval = 12f;
    [SerializeField] private float powerupLifetime = 5f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] powerupPrefabs;

    private float timer = 0f;
    private GameObject currentPowerup = null;

    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;

        // Si hay un potenciador activo en el mapa, no contar tiempo
        if (currentPowerup != null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnPowerup();
        }
    }

    private void SpawnPowerup()
    {
        if (spawnPoints.Length == 0 || powerupPrefabs.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
        currentPowerup = Instantiate(prefab, point.position, Quaternion.identity);

        // Auto-destruccion despues de powerupLifetime segundos
        Destroy(currentPowerup, powerupLifetime);
    }
}