public class ProximityPrompt : MonoBehaviour
{
    public string promptMessage = "Hold SHIFT to bite/grab & release to drop";
    public string requiredTag = "Dog";
    public GameObject popupPanel;

    private bool dogInRange = false;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(requiredTag)) {
            dogInRange = true;
            popupPanel.SetActive(true);
            // Set prompt text here
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag(requiredTag)) {
            dogInRange = false;
            popupPanel.SetActive(false);
        }
    }

    void Update() {
        if (!dogInRange) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            GetComponent<PaperBite>()?.PickUp();

        if (Input.GetKeyUp(KeyCode.LeftShift))
            GetComponent<PaperBite>()?.Drop();
    }
}