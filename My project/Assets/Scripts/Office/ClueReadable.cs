using UnityEngine;

// Attach to Clue_Postcard, Clue_Diary, Clue_DivorceDoc, Clue_Stickynote.
// Replicates PaperReadController + PaperProximityTrigger for Ghost (tag "Player").
public class ClueReadable : MonoBehaviour
{
    public GameObject clueCanvas;
    public GameObject promptPanel;  // 直接拖入提示 Panel，不需要 ProximityPrompt

    private bool playerInRange;
    private bool isOpen;
    [TextArea]
    public string monologueLine;

    void Start()
    {
        if (clueCanvas != null) clueCanvas.SetActive(false);
        if (promptPanel != null) promptPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[ClueReadable] TriggerEnter: {other.gameObject.name} tag={other.tag}");
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        Debug.Log($"[ClueReadable] Player entered range. isOpen={isOpen}");
        if (!isOpen && promptPanel != null)
        {
            Debug.Log("[ClueReadable] Player in range, showing prompt.");
            promptPanel.SetActive(true);
        }
        else if (promptPanel == null)
        {
            Debug.LogWarning("[ClueReadable] promptPanel is not assigned!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"[ClueReadable] TriggerExit: {other.gameObject.name} tag={other.tag}");
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
                CloseClue();
            else
                OpenClue();
        }
       
    }

    void OpenClue()
    {
        isOpen = true;
        if (promptPanel != null) promptPanel.SetActive(false);
        if (clueCanvas != null) clueCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!string.IsNullOrEmpty(monologueLine))
        {
            MonologueManager.Instance.ShowLine(monologueLine);
        }
        
    }

    void CloseClue()
    {
        isOpen = false;
        if (clueCanvas != null) clueCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerInRange && promptPanel != null)
            promptPanel.SetActive(true);
        MonologueManager.Instance.HideLine();
    }
}
