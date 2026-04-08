public class PaperBite : MonoBehaviour
{
    private Rigidbody rb;
    private Transform dogMouth;
    private bool isCarried = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
        dogMouth = GameObject.Find("DogMouth")?.transform;
    }

    public void PickUp() {
        if (dogMouth == null || isCarried) return;
        isCarried = true;
        rb.isKinematic = true;             // disable physics while held
        transform.SetParent(dogMouth);     // snap to mouth
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop() {
        if (!isCarried) return;
        isCarried = false;
        transform.SetParent(null);      
        rb.isKinematic = false;            // re-enable physics 
        GameStateManager.hasPaperCode = true;
    }
}