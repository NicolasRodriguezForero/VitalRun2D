using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameActive = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameActive = true;
        GameTimer.Instance.StartTimer();
        // Aquí se activará spawning cuando exista
    }

    public void EndGame()
    {
        gameActive = false;
        GameTimer.Instance.StopTimer();
        Debug.Log("¡Juego terminado!");
        // Aquí mostraremos la pantalla de resultados en Paso 4
    }

    public bool IsGameActive()
    {
        return gameActive;
    }
}