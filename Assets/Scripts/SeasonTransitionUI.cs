using UnityEngine;
using TMPro;
using System.Collections;

public class SeasonTransitionUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI seasonText;
    public TextMeshProUGUI hintText;

    public float fadeDuration = 0.5f;
    public float holdDuration = 1.2f;

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnSeasonChanged += PlayTransition;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnSeasonChanged -= PlayTransition;
    }

    private void PlayTransition(Season season)
    {
        StopAllCoroutines();
        StartCoroutine(TransitionRoutine(season));
    }

    private IEnumerator TransitionRoutine(Season season)
    {
        canvasGroup.alpha = 0f;

        seasonText.text = $"{season} Begins";
        hintText.text = season switch
        {
            Season.Summer => "Water prices increase",
            Season.Spring => "Seeds are cheaper",
            _ => ""
        };

        // Pause gameplay
        Time.timeScale = 0f;

        // Fade in
        yield return Fade(0f, 1f);

        // Hold
        yield return new WaitForSecondsRealtime(holdDuration);

        // Fade out
        yield return Fade(1f, 0f);

        // Resume gameplay
        Time.timeScale = 1f;
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
