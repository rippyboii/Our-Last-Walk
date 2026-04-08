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
    rb.isKinematic = false;

    // Add a small forward impulse on release
    Vector3 throwDir = dogMouth.forward + Vector3.up * 0.3f;
    rb.AddForce(throwDir * 2f, ForceMode.Impulse);

    GameStateManager.hasPaperCode = true;
}
}