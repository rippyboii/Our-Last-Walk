using UnityEngine;

public class PaperBite : MonoBehaviour

{
    private Rigidbody rb;
    private Transform dogMouth;
    private bool isCarried = false;
    private MeshCollider meshCol;

    public float minY = 0.1f; 

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
        transform.rotation = Quaternion.Euler(270f, 0f, 0f); 
        rb.isKinematic = false;
        if (meshCol != null) meshCol.enabled = true;
        GameStateManager.Instance.hasPaperCode = true;
    } 

    void LateUpdate() {
        if (isCarried) return;
        Vector3 pos = transform.position;
        if (pos.y < minY) {
            pos.y = minY;
            transform.position = pos;
            // kill downward velocity so it doesn't keep pushing through
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }
}
}