using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pausePanel;     // El PausePanel
    public Slider volumeSlider;       // El VolumeSlider

    private bool isPaused = false;

    void Start()
    {
        // Asegurar que arranca despausado y el panel oculto
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // Inicializar el slider con el volumen actual
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;   // congela TODO el juego de fondo
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;   // reanuda el juego
    }

    // Enlazar al botón "Volver al menú" del PausePanel en el Inspector
    public void GoToMainMenu()
    {
        SceneController.LoadMainMenu();   // SceneController ya restaura timeScale
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}