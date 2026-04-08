using UnityEngine;

public class PhoneProximityTrigger : MonoBehaviour
{
    public GameObject promptUI;         
    public PhoneController phone;       

    private bool playerInRange = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (promptUI != null) promptUI.SetActive(false);
            phone.OnInteract();
        }
    }
}