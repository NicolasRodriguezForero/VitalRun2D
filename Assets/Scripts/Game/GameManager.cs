using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ResultsUI resultsUI;

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
    }

    public void EndGame()
    {
        gameActive = false;
        GameTimer.Instance.StopTimer();

        int finalScore = ScoreManager.Instance != null ? ScoreManager.Instance.totalScore : 0;
        int finalOrders = OrderManager.Instance != null ? OrderManager.Instance.CompletedOrders : 0;

        resultsUI.ShowResults(finalScore, finalOrders);
    }

    public bool IsGameActive()
    {
        return gameActive;
    }
}