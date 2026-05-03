using UnityEngine;

// Attach to Clue_Postcard, Clue_Diary, Clue_DivorceDoc, Clue_Stickynote.
// Replicates PaperReadController + PaperProximityTrigger for Ghost (tag "Player").
public class ClueReadable : MonoBehaviour
{
    public GameObject clueCanvas;
    public ProximityPrompt prompt;

    private bool playerInRange;
    private bool isOpen;

    void Start()
    {
        if (clueCanvas != null) clueCanvas.SetActive(false);
        if (prompt != null && prompt.popupPanel != null) prompt.popupPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        if (!isOpen && prompt != null && prompt.popupPanel != null)
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
        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (isOpen)
            CloseClue();
        else if (playerInRange)
            OpenClue();
    }

    void OpenClue()
    {
        isOpen = true;
        if (prompt != null && prompt.popupPanel != null) prompt.popupPanel.SetActive(false);
        if (clueCanvas != null) clueCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseClue()
    {
        isOpen = false;
        if (clueCanvas != null) clueCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerInRange && prompt != null && prompt.popupPanel != null)
            prompt.popupPanel.SetActive(true);
    }
}
