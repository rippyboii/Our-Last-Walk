using UnityEngine;

// Attach to Photo_Dating, Photo_Child, Photo_Divorce, Photo_Dog.
// Ghost picks up the photo with E; while held, WallFrameSlot handles placement.
public class PhotoDrag : MonoBehaviour
{
    public string photoId;           // "dating", "child", "divorce", "dog"
    public ProximityPrompt prompt;   // assign in Inspector

    // Shared across all photos so only one can be held at a time.
    public static PhotoDrag HeldPhoto;
    public static int framesInRange;  // incremented/decremented by WallFrameSlot

    [HideInInspector] public bool enteredFrameZone;  // set by WallFrameSlot on first enter

    private bool playerInRange;
    private bool isHeld;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private Transform holdPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (prompt != null && prompt.popupPanel != null)
            prompt.popupPanel.SetActive(false);

        // Locate PhotoHoldPoint on the Ghost prefab at runtime.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Transform hp = player.transform.Find("PhotoHoldPoint");
            if (hp != null) holdPoint = hp;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isHeld || HeldPhoto != null) return;
        playerInRange = true;
        if (prompt != null && prompt.popupPanel != null)
            prompt.popupPanel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (prompt != null && prompt.popupPanel != null)
            prompt.popupPanel.SetActive(false);
    }

    void Update()
    {
        // Auto-drop: Ghost entered at least one frame zone but has now left all of them.
        if (isHeld && enteredFrameZone && framesInRange == 0)
        {
            Drop();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHeld && playerInRange && HeldPhoto == null)
            PickUp();
    }

    void PickUp()
    {
        isHeld = true;
        HeldPhoto = this;
        framesInRange = 0;
        enteredFrameZone = false;

        rb.isKinematic = true;
        rb.detectCollisions = false;

        if (holdPoint != null)
        {
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        if (prompt != null && prompt.popupPanel != null)
            prompt.popupPanel.SetActive(false);
        playerInRange = false;
    }

    // Called by WallFrameSlot after successfully snapping photo into the frame.
    public void PlacedInFrame()
    {
        isHeld = false;
        if (HeldPhoto == this) HeldPhoto = null;
        enteredFrameZone = false;
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    // Called by WallFrameSlot when Ghost removes a placed photo (picks it back up).
    public void PickUpFromFrame()
    {
        isHeld = true;
        HeldPhoto = this;
        // Ghost is still inside the frame zone that triggered the removal.
        framesInRange = 1;
        enteredFrameZone = true;

        rb.isKinematic = true;
        rb.detectCollisions = false;

        if (holdPoint != null)
        {
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public void Drop()
    {
        isHeld = false;
        if (HeldPhoto == this) HeldPhoto = null;
        enteredFrameZone = false;

        transform.SetParent(null);
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
}
