using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Fades a black UI overlay before and after scene changes.
    /// </summary>
    public class SceneFader : MonoBehaviour
    {
        public static SceneFader Instance { get; private set; }

        [SerializeField] private CanvasGroup fadeOverlay;
        [SerializeField] private float fadeDuration = 0.35f;

        private bool loadingScene;

        private void Awake()
        {
            Instance = this;
            fadeOverlay.alpha = 1f;
            fadeOverlay.blocksRaycasts = true;
        }

        private IEnumerator Start()
        {
            yield return FadeTo(0f);
            fadeOverlay.blocksRaycasts = false;
        }

        public void LoadScene(string sceneName)
        {
            if (!loadingScene)
            {
                StartCoroutine(FadeOutAndLoad(sceneName));
            }
        }

        public void ReloadCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        private IEnumerator FadeOutAndLoad(string sceneName)
        {
            loadingScene = true;
            fadeOverlay.blocksRaycasts = true;
            yield return FadeTo(1f);
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }

        private IEnumerator FadeTo(float targetAlpha)
        {
            float startingAlpha = fadeOverlay.alpha;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeOverlay.alpha = Mathf.Lerp(
                    startingAlpha, targetAlpha, elapsed / fadeDuration);
                yield return null;
            }

            fadeOverlay.alpha = targetAlpha;
        }

        // Used only by the editor scene builder.
        public void Configure(CanvasGroup overlay)
        {
            fadeOverlay = overlay;
        }
    }
}
