using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fade negro entre escenas. Vive en un prefab con DontDestroyOnLoad.
/// SceneController lo instancia y llama a FadeOut / FadeIn.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.35f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Oscurece la pantalla a negro (0 → 1).</summary>
    public IEnumerator FadeOut()
    {
        canvasGroup.blocksRaycasts = true;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    /// <summary>Aclara la pantalla desde negro (1 → 0).</summary>
    public IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}
