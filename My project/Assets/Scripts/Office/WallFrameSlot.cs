using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Attach to WallFrame_1 … WallFrame_4.
// Handles placement, removal, and highlight feedback for one wall frame.
public class WallFrameSlot : MonoBehaviour
{
    public int slotIndex;                     // 0 = leftmost
    public PhotoPuzzleManager puzzleManager;
    public MeshRenderer frameMeshRenderer;    // the frame's own MeshRenderer (element 1 = photo slot)

    // Optional prompt UI for this slot (separate from the photo's ProximityPrompt).
    public GameObject promptUI;
    public TMP_Text promptText;

    [HideInInspector] public string currentPhotoId = "";

    private bool playerInRange;
    private PhotoDrag photoInSlot;
    private bool locked;

    private Renderer[] frameRenderers;
    private int defaultLayer;
    private int highlightLayer;
    private Material originalSlot1Material;

    void Start()
    {
        frameRenderers = GetComponentsInChildren<Renderer>();
        defaultLayer = LayerMask.NameToLayer("Default");
        highlightLayer = LayerMask.NameToLayer("Highlighted");

        if (frameMeshRenderer != null && frameMeshRenderer.materials.Length > 1)
            originalSlot1Material = frameMeshRenderer.materials[1];

        SetHighlight(false);
        if (promptUI != null) promptUI.SetActive(false);

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[WallFrameSlot {slotIndex}] TriggerEnter: {other.name} tag={other.tag}, HeldPhoto={PhotoDrag.HeldPhoto?.photoId ?? "null"}");
        if (!other.CompareTag("Player")) return;
        playerInRange = true;

        if (PhotoDrag.HeldPhoto != null)
        {
            // Register this zone with the held photo.
            PhotoDrag.framesInRange++;
            PhotoDrag.HeldPhoto.enteredFrameZone = true;

            SetHighlight(true);
            ShowPrompt("Press E to place");
        }
        else if (currentPhotoId != "")
        {
            ShowPrompt("Press E to take back");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;

        if (PhotoDrag.HeldPhoto != null && PhotoDrag.framesInRange > 0)
            PhotoDrag.framesInRange--;

        SetHighlight(false);
        HidePrompt();
    }

    void Update()
    {
        if (!playerInRange || locked || !Input.GetKeyDown(KeyCode.E)) return;

        if (PhotoDrag.HeldPhoto != null && currentPhotoId == "")
            PlacePhoto(PhotoDrag.HeldPhoto);
        else if (PhotoDrag.HeldPhoto == null && currentPhotoId != "" && photoInSlot != null)
            RemovePhoto();
    }

    void PlacePhoto(PhotoDrag photo)
    {
        currentPhotoId = photo.photoId;
        photoInSlot = photo;

        photo.PlacedInFrame();

        if (frameMeshRenderer != null && photo.photoMaterial != null)
        {
            Material[] mats = frameMeshRenderer.materials;
            mats[1] = photo.photoMaterial;
            frameMeshRenderer.materials = mats;
        }

        SetHighlight(false);
        HidePrompt();
        puzzleManager.CheckSolution();
    }

    void RemovePhoto()
    {
        PhotoDrag removed = photoInSlot;
        currentPhotoId = "";
        photoInSlot = null;

        if (frameMeshRenderer != null && originalSlot1Material != null)
        {
            Material[] mats = frameMeshRenderer.materials;
            mats[1] = originalSlot1Material;
            frameMeshRenderer.materials = mats;
        }

        removed.PickUpFromFrame();
        ShowPrompt("Press E to place");
        SetHighlight(true);
        puzzleManager.CheckSolution();
    }

    public void SetLocked(bool value)
    {
        locked = value;
    }

    void ShowPrompt(string message)
    {
        if (promptUI == null) return;
        if (promptText != null) promptText.text = message;
        promptUI.SetActive(true);
    }

    void HidePrompt()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    void SetHighlight(bool on)
{
    int layer = on ? highlightLayer : defaultLayer;
    foreach (var r in frameRenderers)
        r.gameObject.layer = layer;
}
}
