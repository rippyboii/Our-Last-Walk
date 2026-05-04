using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Attach to WallFrame_1 … WallFrame_4.
// Handles placement, removal, and highlight feedback for one wall frame.
public class WallFrameSlot : MonoBehaviour
{
    public int slotIndex;                     // 0 = leftmost
    public PhotoPuzzleManager puzzleManager;
    public GameObject highlightVisual;        // child outline object, starts inactive

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

    void Start()
    {
        frameRenderers = GetComponentsInChildren<Renderer>();
        defaultLayer = LayerMask.NameToLayer("Default");      
        highlightLayer = LayerMask.NameToLayer("Highlighted"); 
        
        SetHighlight(false);
        if (promptUI != null) promptUI.SetActive(false);

    }

    void OnTriggerEnter(Collider other)
    {
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
        photo.transform.SetParent(transform);
        photo.transform.localPosition = Vector3.zero;
        photo.transform.localRotation = Quaternion.identity;

        SetHighlight(false);
        HidePrompt();
        puzzleManager.CheckSolution();
    }

    void RemovePhoto()
    {
        PhotoDrag removed = photoInSlot;
        currentPhotoId = "";
        photoInSlot = null;

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
