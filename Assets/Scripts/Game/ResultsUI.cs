using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI ordersCompletedText;

    void Start()
    {
        resultsPanel.SetActive(false);
    }

    public void ShowResults(int score, int orders)
    {
        resultsPanel.SetActive(true);
        finalScoreText.text = "Puntaje: " + score;
        ordersCompletedText.text = "Órdenes completadas: " + orders;
    }

    public void OnPlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}