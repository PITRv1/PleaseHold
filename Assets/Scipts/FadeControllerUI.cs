using UnityEngine;
using System.Collections;

public class FadeControllerUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOut(float duration)
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }
        StartCoroutine(FadeCanvasGroup(1, 0, duration, () => gameObject.SetActive(false)));
    }

    public void FadeIn(float duration)
    {
        if (gameObject.activeSelf == true) {
            return;
        }
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(0, 1, duration, null));
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float duration, System.Action onComplete)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        onComplete?.Invoke();
    }
}
