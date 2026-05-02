using UnityEngine;

public class QTETriggerZone : MonoBehaviour
{
    private QTEPrompt qtePrompt;

    void Start()
    {
        qtePrompt = FindObjectOfType<QTEPrompt>();
    }

    void OnTriggerEnter(Collider other)
    {
        // only fire if it's the dog walking through
        if (other.GetComponent<Dog_movement>() == null) return;
        qtePrompt.TriggerQTE();
    }
}