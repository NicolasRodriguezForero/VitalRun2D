using UnityEngine;

/// <summary>
/// Controla los botones del Menú Principal.
/// Adjuntar a un GameObject vacío "MenuManager" en MainMenu.unity.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    public void OnPlay()
    {
        SceneController.LoadGame();
    }

    public void OnTutorial()
    {
        SceneController.LoadTutorial();
    }

    public void OnQuit()
    {
        SceneController.Quit();
    }
}
