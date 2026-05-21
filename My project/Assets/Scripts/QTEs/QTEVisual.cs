using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEVisual : MonoBehaviour
{
    [Header("References")]
    public GameObject qtePanel;           // parent object to show/hide
    public Image outerCircle;            // the shrinking ring
    public Image successZoneIndicator;   // shows where the success zone is
    public TextMeshProUGUI keyText;      // the letter in the middle

    private QTEPrompt qtePrompt;
    private bool isShowing = false;

    void Start()
    {
        qtePrompt = FindObjectOfType<QTEPrompt>();
        qtePrompt.OnSuccess += ShowSuccess;
        qtePrompt.OnFail += ShowFail;
        qtePanel.SetActive(false);
    }

void Update()
{
    if (!isShowing) return;

    float progress = qtePrompt.GetProgress();

    // drain the circle as time runs out
    outerCircle.fillAmount = 1f - progress;

    // color feedback
    if (progress >= qtePrompt.GetSuccessStart() && 
        progress <= qtePrompt.GetSuccessEnd())
        outerCircle.color = Color.green;
    else if (progress > qtePrompt.GetSuccessEnd())
        outerCircle.color = Color.red;
    else
        outerCircle.color = Color.white;
}

    public void Show(KeyCode key)
    {
        isShowing = true;
        qtePanel.SetActive(true);
        keyText.text = key.ToString();
        outerCircle.fillAmount = 1f; // reset to full when showing
        outerCircle.color = Color.white;
    }

    public void ShowSuccess()
    {
        isShowing = false;
        // TODO: brief green flash then hide
        qtePanel.SetActive(false);
    }

    public void ShowFail()
    {
        isShowing = false;
        // TODO: brief red flash then hide
        qtePanel.SetActive(false);
    }

    void OnDestroy()
    {
        qtePrompt.OnSuccess -= ShowSuccess;
        qtePrompt.OnFail -= ShowFail;
    }
}