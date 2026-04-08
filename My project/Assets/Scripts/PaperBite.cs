using UnityEngine;

public class PaperBite : MonoBehaviour
{
    private Rigidbody rb;
    private Transform dogMouth;
    private bool isCarried = false;
    private MeshCollider meshCol;

    void Start() {
        rb = GetComponent<Rigidbody>();
        dogMouth = GameObject.Find("DogMouth")?.transform;
        meshCol = GetComponent<MeshCollider>();
    }

    public void PickUp() {
        if (dogMouth == null || isCarried) return;
        isCarried = true;
        rb.isKinematic = true;
        if (meshCol != null) meshCol.enabled = false; // stop it pushing the dog
        transform.SetParent(dogMouth);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop() {
        if (!isCarried) return;
        isCarried = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        if (meshCol != null) meshCol.enabled = true; // re-enable on drop

        Vector3 throwDir = dogMouth.forward + Vector3.up * 0.3f;
        rb.AddForce(throwDir * 2f, ForceMode.Impulse);

        GameStateManager.Instance.hasPaperCode = true;
    }
}