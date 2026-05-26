using UnityEngine;
using System.Collections;

public class SceneStarter : MonoBehaviour
{
    [TextArea]
    public string openingLine;
    public float delay = 1f; // small delay so it doesn't pop instantly

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(ShowOpeningLine());
    }

    IEnumerator ShowOpeningLine()
    {
        yield return new WaitForSeconds(delay);
        MonologueManager.Instance.ShowLine(openingLine);
        yield return new WaitForSeconds(2f); 
        MonologueManager.Instance.HideLine();
    }
}