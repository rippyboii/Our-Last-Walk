using UnityEngine;

public class PhoneProximityTrigger : MonoBehaviour
{
    public GameObject promptUI;
    public PhoneController phone;

    private bool dogInRange = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog")) 
        {
            dogInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            dogInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (dogInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (promptUI != null) promptUI.SetActive(false);
            phone.OnInteract();
        }
    }
}