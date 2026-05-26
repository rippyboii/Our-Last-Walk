using UnityEngine;
using TMPro;
using System.Collections;

// Attach to a single GameObject in the scene - manages all monologue UI
public class MonologueManager : MonoBehaviour
{
    public static MonologueManager Instance;

    [Header("UI References")]
    public CanvasGroup monologuePanel;
    public TextMeshProUGUI monologueText;

    [Header("Settings")]
    public float fadeDuration = 0.5f;

    void Awake()
    {
        Instance = this;
        monologuePanel.gameObject.SetActive(false);
    }

    public void ShowLine(string line)
    {
        Debug.Log($"[MonologueManager] ShowLine: {line}");
        StopAllCoroutines();
        StartCoroutine(FadeIn(line));
    }

    public void HideLine()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn(string line)
    {
        monologueText.text = line;
        monologuePanel.gameObject.SetActive(true);
        monologuePanel.alpha = 0f;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // ← fix
            monologuePanel.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float timer = fadeDuration;
        while (timer > 0f)
        {
            timer -= Time.unscaledDeltaTime; // ← fix
            monologuePanel.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        monologuePanel.gameObject.SetActive(false);
    }
}