using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Punto central de navegación entre escenas.
/// Todos los botones del juego (MainMenu, Pausa, Resultados, Tutorial)
/// llaman a métodos estáticos de esta clase.
/// Ejecuta un fade negro antes y después de cada carga.
/// </summary>
public class SceneController : MonoBehaviour
{
    // Nombres de escena (deben coincidir con Build Settings)
    private const string SCENE_MAIN_MENU = "MainMenu";
    private const string SCENE_TUTORIAL  = "Tutorial";
    private const string SCENE_GAME      = "Game";

    [Header("Prefab del fader (asignar en Inspector)")]
    [SerializeField] private SceneFader faderPrefab;

    // Instancia singleton para poder lanzar coroutines
    private static SceneController instance;

    // Se auto-crea si no existe (permite entrar desde cualquier escena)
    private static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("SceneController");
                instance = go.AddComponent<SceneController>();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ── Métodos públicos (llamar desde botones) ───────────────────────────────

    public static void LoadMainMenu() => Instance.StartLoad(SCENE_MAIN_MENU);
    public static void LoadTutorial() => Instance.StartLoad(SCENE_TUTORIAL);
    public static void LoadGame()     => Instance.StartLoad(SCENE_GAME);
    public static void ReloadGame()   => Instance.StartLoad(SceneManager.GetActiveScene().name);

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ── Lógica interna ────────────────────────────────────────────────────────

    private void StartLoad(string sceneName)
    {
        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        // 1. Obtener o crear el fader
        SceneFader fader = FindAnyObjectByType<SceneFader>();
        if (fader == null && faderPrefab != null)
            fader = Instantiate(faderPrefab);

        // 2. Restaurar timeScale (por si venimos de Pausa o Resultados)
        Time.timeScale = 1f;

        // 3. Fade a negro
        if (fader != null)
            yield return StartCoroutine(fader.FadeOut());

        // 4. Cargar escena
        yield return SceneManager.LoadSceneAsync(sceneName);

        // 5. Fade desde negro
        if (fader != null)
            yield return StartCoroutine(fader.FadeIn());
    }
}
