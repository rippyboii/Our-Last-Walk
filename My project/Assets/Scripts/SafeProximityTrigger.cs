using UnityEngine;

public class SafeProximityTrigger : MonoBehaviour
{
    public GameObject promptUI;
    public SafeController safe; 
    public string playerTag = "Dog";

    private bool dogInRange = false;

    void Start() { if (promptUI != null) promptUI.SetActive(false); }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag)) { dogInRange = true; promptUI?.SetActive(true); }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag)) { dogInRange = false; promptUI?.SetActive(false); }
    }

    void Update()
    {
        if (dogInRange && Input.GetKeyDown(KeyCode.E))
        {
            promptUI?.SetActive(false);
            safe.OpenSafe();
        }
    }
}