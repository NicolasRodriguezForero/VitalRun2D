using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pausePanel;     // El PausePanel
    public Slider volumeSlider;       // El VolumeSlider

    [Header("Escenas")]
    public string mainMenuSceneName = "MainMenu";

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

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;   // MUY importante: restaurar antes de cambiar de escena
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}