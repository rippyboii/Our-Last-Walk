using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach to the FlashbackManager GameObject.
// Plays 4-5 comic panels then reveals the laptop password date.
public class FlashbackManager : MonoBehaviour
{
    public static FlashbackManager Instance;

    [Header("Comic Panels")]
    public Sprite[] panels;           // 4-5 sprites, assign in Inspector
    public Image panelImage;          // UI Image that displays the current panel

    [Header("Date Reveal")]
    public string revealedDate;       // must match GameStateManager.Instance.laptopPassword
    public GameObject dateScreen;     // child panel showing the date text
    public TMP_Text dateText;             // displays revealedDate

    [Header("Canvas")]
    public GameObject flashbackCanvas;

    [HideInInspector] public bool flashbackSeen;

    private bool isPlaying;
    private int currentPanel;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (flashbackCanvas != null) flashbackCanvas.SetActive(false);
        if (dateScreen != null) dateScreen.SetActive(false);
    }

    void Update()
    {
        if (!isPlaying || !Input.GetKeyDown(KeyCode.E)) return;

        if (currentPanel < panels.Length - 1)
        {
            currentPanel++;
            ShowPanel(currentPanel);
        }
        else if (currentPanel == panels.Length - 1)
        {
            // Last comic panel — advance to date screen.
            currentPanel = panels.Length;
            ShowDateScreen();
        }
        else
        {
            // Date screen — close flashback.
            CloseFlashback();
        }
    }

    public void PlayFlashback()
    {
        if (panels == null || panels.Length == 0) return;

        isPlaying = true;
        currentPanel = 0;

        flashbackCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (dateScreen != null) dateScreen.SetActive(false);
        ShowPanel(0);
    }

    void ShowPanel(int index)
    {
        if (panelImage == null) return;
        panelImage.gameObject.SetActive(true);
        panelImage.sprite = panels[index];
        if (dateScreen != null) dateScreen.SetActive(false);
    }

    void ShowDateScreen()
    {
        if (panelImage != null) panelImage.gameObject.SetActive(false);
        if (dateText != null) dateText.text = revealedDate;
        if (dateScreen != null) dateScreen.SetActive(true);
    }

    void CloseFlashback()
    {
        isPlaying = false;
        flashbackSeen = true;
        flashbackCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
