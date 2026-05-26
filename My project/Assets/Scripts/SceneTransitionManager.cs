using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Token Animation")]
    public GameObject tokenPanel;
    public Image tokenImage;
    public CanvasGroup tokenCanvasGroup;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip thudSound;

    [Header("Settings")]
    public string nextSceneName;
    public float tokenDisplayTime = 3f;
    public float fadeDuration = 0.8f;

    private bool triggered = false;

    void Start()
    {
        tokenPanel.SetActive(false);
    }

    public void TriggerTransition()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(TransitionSequence());
    }

    IEnumerator TransitionSequence()
    {
        // show token panel
        tokenPanel.SetActive(true);
        tokenCanvasGroup.alpha = 0f;

        // fade in token
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            tokenCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        // play thud sound when fully visible
        if (audioSource != null && thudSound != null)
            audioSource.PlayOneShot(thudSound);

        // token scale pop animation
        // tokenImage.transform.localScale = Vector3.one * 0.8f;
        // timer = 0f;
        // while (timer < 0.3f)
        // {
        //     timer += Time.unscaledDeltaTime;
        //     float scale = Mathf.Lerp(0.8f, 1.1f, timer / 0.3f);
        //     tokenImage.transform.localScale = Vector3.one * scale;
        //     yield return null;
        // }
        // // settle back to normal size
        // timer = 0f;
        // while (timer < 0.15f)
        // {
        //     timer += Time.unscaledDeltaTime;
        //     float scale = Mathf.Lerp(1.1f, 1f, timer / 0.15f);
        //     tokenImage.transform.localScale = Vector3.one * scale;
        //     yield return null;
        // }

        // wait for player to read
        yield return new WaitForSecondsRealtime(tokenDisplayTime);
        // play thud sound when fully visible
        

        // fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            tokenCanvasGroup.alpha = 1f - Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        if (audioSource != null && thudSound != null)
            audioSource.PlayOneShot(thudSound);
        
        SceneManager.LoadScene(nextSceneName);
    }
}