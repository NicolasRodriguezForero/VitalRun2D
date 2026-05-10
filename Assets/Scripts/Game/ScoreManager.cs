using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Estado")]
    public int totalScore = 0;
    public int currentCombo = 0;

    [Header("Multiplicadores")]
    public bool doublePointsActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddPoints(OrderData order)
    {
        int points = order.basePoints;

        // Aplicar combo
        currentCombo++;
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
            Debug.Log("Combo perdido");
            currentCombo = 0;
        }
    }
}