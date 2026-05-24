using UnityEngine;

public class TelephoneProximityTrigger : MonoBehaviour
{
    public GameObject promptUI;
    public TelephoneController telephone;

    private bool playerInRange = false;

    void Start() { if (promptUI != null) promptUI.SetActive(false); }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { playerInRange = true; promptUI?.SetActive(true); }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { playerInRange = false; promptUI?.SetActive(false); }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            promptUI?.SetActive(false);
            telephone.EnterTelephoneMode();
        }
    }
}