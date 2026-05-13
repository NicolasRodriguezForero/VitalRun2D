using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private float totalTime = 120f;
    [SerializeField] private float warningTime = 30f;

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning = false;

    private Color normalColor = Color.white;
    private Color warningColor = Color.red;
    private float blinkInterval = 0.5f;
    private float blinkTimer = 0f;
    private bool blinkOn = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = totalTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            UpdateTimerDisplay();
            GameManager.Instance.EndGame();
            return;
        }

        // Parpadeo en los últimos 30 segundos
        if (timeRemaining <= warningTime)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                blinkTimer = 0f;
                blinkOn = !blinkOn;
            }
            timerText.color = blinkOn ? warningColor : Color.clear;
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        timeRemaining = totalTime;
        timerText.color = normalColor;
        blinkOn = true;
        blinkTimer = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void AddTime(float seconds)
    {
        timeRemaining += seconds;

        // Si la suma saca el tiempo del rango de warning, restaurar color
        if (timeRemaining > warningTime)
        {
            timerText.color = normalColor;
            blinkOn = true;
            blinkTimer = 0f;
        }

        UpdateTimerDisplay();
    }
}