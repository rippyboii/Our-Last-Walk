using UnityEngine;
using System.Collections;

public class QTEPrompt : MonoBehaviour
{
    [Header("Settings")]
    public float timeWindow = 2f;          // total time the prompt is shown
    public float successZoneStart = 0.3f;  // earliest the player can press (0-1)
    public float successZoneEnd = 0.7f;    // latest the player can press (0-1)
    public KeyCode promptKey = KeyCode.E;

    // events for PickUp script to listen to
    public event System.Action OnSuccess;
    public event System.Action OnFail;

    private bool isActive = false;
    private float timer = 0f;             // 0 = just started, 1 = time ran out
    private bool hasPressed = false;
    private PickUp pickUp;
    private QTEVisual qteVisual;

    void Start()
    {
        pickUp = FindObjectOfType<PickUp>();
        qteVisual = FindObjectOfType<QTEVisual>();
    }

    void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime / timeWindow; // normalize to 0-1

        // ran out of time
        if (timer >= 1f)
        {
            TriggerFail();
            return;
        }

        if (Input.GetKeyDown(promptKey) && !hasPressed)
        {
            hasPressed = true;

            // check if pressed in the success zone
            if (timer >= successZoneStart && timer <= successZoneEnd)
                TriggerSuccess();
            else
                TriggerFail(); // pressed too early or too late
        }
    }

    public void TriggerQTE()
    {
        if (pickUp.currentlyCarried == null) return; // only fire if carrying
        isActive = true;
        timer = 0f;
        hasPressed = false;
        qteVisual.Show(promptKey);
    }

    void TriggerSuccess()
    {
        isActive = false;
        // TODO: tell UI to show success feedback
        OnSuccess?.Invoke();
        qteVisual.ShowSuccess();
    }

    void TriggerFail()
    {
        isActive = false;
        // TODO: tell UI to hide
        OnFail?.Invoke();
        qteVisual.ShowFail();
    }

    // expose timer for UI to read
    public float GetProgress() => timer;
    public float GetSuccessStart() => successZoneStart;
    public float GetSuccessEnd() => successZoneEnd;
}