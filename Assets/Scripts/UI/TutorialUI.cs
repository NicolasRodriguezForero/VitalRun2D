using UnityEngine;

/// <summary>
/// Controla la pantalla de Tutorial.
/// Adjuntar a un GameObject "TutorialManager" en Tutorial.unity.
/// Soporta una o varias páginas/imágenes de tutorial.
/// </summary>
public class TutorialUI : MonoBehaviour
{
    [Header("Páginas del tutorial (opcional)")]
    [Tooltip("Arrastra aquí cada panel/imagen de tutorial en orden. Si solo hay uno, déjalo vacío.")]
    [SerializeField] private GameObject[] paginas;

    private int paginaActual = 0;

    void Start()
    {
        MostrarPagina(0);
    }

    // ── Navegación ────────────────────────────────────────────────────────────

    public void OnNextPage()
    {
        if (paginas == null || paginas.Length == 0) return;
        MostrarPagina(paginaActual + 1);
    }

    public void OnPrevPage()
    {
        if (paginas == null || paginas.Length == 0) return;
        MostrarPagina(paginaActual - 1);
    }

    // ── Botón principal ───────────────────────────────────────────────────────

    public void OnBackToMenu()
    {
        SceneController.LoadMainMenu();
    }

    // ── Lógica interna ────────────────────────────────────────────────────────

    private void MostrarPagina(int index)
    {
        if (paginas == null || paginas.Length == 0) return;

        paginaActual = Mathf.Clamp(index, 0, paginas.Length - 1);

        for (int i = 0; i < paginas.Length; i++)
        {
            if (paginas[i] != null)
                paginas[i].SetActive(i == paginaActual);
        }
    }
}
