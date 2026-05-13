using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float spawnInterval = 12f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] powerupPrefabs;

    private float timer = 0f;

    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;

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

        // Verificar que no haya ya un potenciador en ese punto
        Collider2D existing = Physics2D.OverlapCircle(point.position, 0.3f);
        if (existing != null && existing.GetComponent<PowerupPickup>() != null)
        {
            return;
        }

        GameObject prefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
        Instantiate(prefab, point.position, Quaternion.identity);
    }
}