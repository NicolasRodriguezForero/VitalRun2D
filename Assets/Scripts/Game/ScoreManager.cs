using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Estado")]
    public int totalScore = 0;
    public int currentCombo = 0;

    [Header("Combo")]
    public float comboTimeLimit = 15f;
    private float timeSinceLastDelivery = 0f;

    [Header("Multiplicadores")]
    public bool doublePointsActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Si hay combo activo, contar el tiempo
        if (currentCombo > 0)
        {
            timeSinceLastDelivery += Time.deltaTime;

            if (timeSinceLastDelivery >= comboTimeLimit)
            {
                BreakCombo();
            }
        }
    }

    public void AddPoints(OrderData order)
    {
        int points = order.basePoints;

        // Aplicar combo
        currentCombo++;
        timeSinceLastDelivery = 0f;

        float multiplier = 1f;
        if (currentCombo >= 5) multiplier = 2f;
        else if (currentCombo >= 3) multiplier = 1.5f;

        // Aplicar doble puntuación si está activo
        if (doublePointsActive)
        {
            multiplier *= 2f;
            doublePointsActive = false;
        }

        int finalPoints = Mathf.RoundToInt(points * multiplier);
        totalScore += finalPoints;

        Debug.Log("+" + finalPoints + " pts (combo: " + currentCombo + ", multiplicador: " + multiplier + "x) | Total: " + totalScore);
    }

    public void BreakCombo()
    {
        if (currentCombo > 0)
        {
            Debug.Log("Combo perdido por inactividad");
            currentCombo = 0;
            timeSinceLastDelivery = 0f;
        }
    }
}