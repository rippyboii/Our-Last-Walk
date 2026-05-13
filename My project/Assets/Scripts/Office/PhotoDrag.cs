using UnityEngine;

// Attach to Photo_Dating, Photo_Child, Photo_Divorce, Photo_Dog.
// On pickup the photo's renderers are hidden; WallFrameSlot shows the photo via frameDisplay.
public class PhotoDrag : MonoBehaviour
{
    public string photoId;           // "dating", "child", "divorce", "dog"
    public GameObject promptPanel;   // the Panel inside your PromptCanvas
    public Material photoMaterial;   // material shown inside the wall frame on placement

    // Shared across all photos so only one can be held at a time.
    public static PhotoDrag HeldPhoto;
    public static int framesInRange;  // incremented/decremented by WallFrameSlot

    [HideInInspector] public bool enteredFrameZone;

    private bool playerInRange;
    private bool isHeld;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private Renderer[] photoRenderers;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        photoRenderers = GetComponentsInChildren<Renderer>();

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isHeld || HeldPhoto != null) return;
        playerInRange = true;
        if (promptPanel != null)
            promptPanel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (promptPanel != null)
            promptPanel.SetActive(false);
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
        SetRenderersVisible(false);

        if (promptPanel != null)
            promptPanel.SetActive(false);
        playerInRange = false;
    }

    // Called by WallFrameSlot after snapping photo into the frame.
    public void PlacedInFrame()
    {
        isHeld = false;
        if (HeldPhoto == this) HeldPhoto = null;
        enteredFrameZone = false;
        // Renderers stay hidden — frameDisplay on WallFrameSlot shows the photo.
    }

    // Called by WallFrameSlot when Ghost removes a placed photo.
    public void PickUpFromFrame()
    {
        isHeld = true;
        HeldPhoto = this;
        framesInRange = 1;
        enteredFrameZone = true;
        // Renderers stay hidden — Ghost holds it invisibly.
    }

    public void Drop()
    {
        isHeld = false;
        if (HeldPhoto == this) HeldPhoto = null;
        enteredFrameZone = false;

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        rb.isKinematic = false;
        rb.detectCollisions = true;
        SetRenderersVisible(true);
    }

    void SetRenderersVisible(bool visible)
    {
        foreach (Renderer r in photoRenderers)
            r.enabled = visible;
    }
}
