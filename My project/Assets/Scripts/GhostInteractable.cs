using UnityEngine;
using System.Collections;

public class GhostInteractable : MonoBehaviour
{
    [Header("Content")]
    [TextArea]
    public string monologueLine;
    public bool flyToCamera = true;

    [Header("Settings")]
    public Transform cameraViewPoint;
    public float flyDuration = 0.5f;

    private bool isOpen = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    public void Interact()
    {
        Debug.Log("Interact called, isOpen: " + isOpen);

        if (isOpen)
            StartCoroutine(Close());
        else
            StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        
        isOpen = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;

        if (flyToCamera)
        {
            transform.SetParent(cameraViewPoint);
            float timer = 0f;
            Vector3 startPos = transform.localPosition;
            Quaternion startRot = transform.localRotation;

            while (timer < flyDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flyDuration;
                transform.localPosition = Vector3.Lerp(startPos, Vector3.zero, t);
                transform.localRotation = Quaternion.Lerp(startRot, Quaternion.identity, t);
                yield return null;
            }
        }

        // tell manager to show text - no direct UI reference needed
        MonologueManager.Instance.ShowLine(monologueLine);
    }

    IEnumerator Close()
    {
        MonologueManager.Instance.HideLine();

        // wait for fade out before flying back
        yield return new WaitForSeconds(MonologueManager.Instance.fadeDuration);

        if (flyToCamera)
        {
            float timer = 0f;
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            transform.SetParent(originalParent);

            while (timer < flyDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flyDuration;
                transform.position = Vector3.Lerp(startPos, originalPosition, t);
                transform.rotation = Quaternion.Lerp(startRot, originalRotation, t);
                yield return null;
            }

            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }

        isOpen = false;
    }
}