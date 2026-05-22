using UnityEngine;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI ordersCompletedText;
    [SerializeField] private TextMeshProUGUI mensajeCierreText;   // opcional

    void Start()
    {
        resultsPanel.SetActive(false);
    }

    public void ShowResults(int score, int orders)
    {
        resultsPanel.SetActive(true);
        finalScoreText.text = "Puntaje: " + score;
        ordersCompletedText.text = "Órdenes completadas: " + orders;

        if (mensajeCierreText != null)
            mensajeCierreText.text = "¡Gracias por tu ayuda!";
    }

    // Enlazar al botón "Jugar de nuevo" en el Inspector
    public void OnPlayAgain()
    {
        SceneController.ReloadGame();
    }

    // Enlazar al botón "Menú principal" en el Inspector
    public void OnMainMenu()
    {
        SceneController.LoadMainMenu();
    }
}