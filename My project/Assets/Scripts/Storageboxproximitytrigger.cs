using UnityEngine;

public class StorageBoxProximityTrigger : MonoBehaviour
{
    public GameObject promptUI;
    public StorageBoxController storageBox;
    public Player Player;

    private bool dogInRange = false;

    void Start() { if (promptUI != null) promptUI.SetActive(false); }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog")) 
        { 
            dogInRange = true; promptUI?.SetActive(true); 
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog")) 
        { 
            dogInRange = false; promptUI?.SetActive(false); 
        }
    }

    void Update()
    {
        if (dogInRange && Input.GetKeyDown(KeyCode.E) && Player.IsDog())
        {
            promptUI?.SetActive(false);
            storageBox?.OpenSlideshow();
        }
    }
}

