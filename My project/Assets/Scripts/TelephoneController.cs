using UnityEngine;

public class TelephoneController : MonoBehaviour
{

    [Header("UI Manager")]
    public TelephoneUIManager uiManager;

    [Header("Camera")]
    public Camera playerCamera;
    public Vector3 cameraOffset   = new Vector3(0f, 0.4f, 0.3f); // tweak in Inspector
    public Vector3 cameraRotation = new Vector3(30f, 180f, 0f);  // looking at phone face

    [Header("Mesh Button")]
    public string messagesBtnName = "messages_btn"; // exact name of the child mesh

    private bool inTelephoneMode = false;

    // Camera state
    private Vector3    camOriginalPosition;
    private Quaternion camOriginalRotation;
    private bool       camStored = false;

    private Camera _cam;

    void Start()
    {
        _cam = playerCamera != null ? playerCamera : Camera.main;
    }

    void Update()
    {
        if (!inTelephoneMode) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTelephoneMode();
            return;
        }

        if (Input.GetMouseButtonDown(0))
            DetectButtonClick();
    }

    public void EnterTelephoneMode()
    {
        inTelephoneMode = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        if (_cam != null)
        {
            if (!camStored)
            {
                camOriginalPosition = _cam.transform.position;
                camOriginalRotation = _cam.transform.rotation;
                camStored = true;
            }

            _cam.transform.position = transform.TransformPoint(cameraOffset);
            _cam.transform.rotation = Quaternion.Euler(cameraRotation);
        }

        DisablePlayerMovement(true);
        uiManager?.OnEnterMode();
    }

    public void ExitTelephoneMode()
    {
        inTelephoneMode = false;

        if (_cam != null && camStored)
        {
            _cam.transform.position = camOriginalPosition;
            _cam.transform.rotation = camOriginalRotation;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        DisablePlayerMovement(false);
        uiManager?.OnExitMode();
    }

    void DetectButtonClick()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if (hit.transform.name.ToLower() == messagesBtnName.ToLower())
                uiManager?.ShowContacts();
        }
    }

    void DisablePlayerMovement(bool disable)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            string n = script.GetType().Name.ToLower();
            if (n.Contains("firstperson") || n.Contains("fps") ||
                n.Contains("player")      || n.Contains("movement") ||
                n.Contains("character"))
            {
                script.enabled = !disable;
            }
        }
    }
}