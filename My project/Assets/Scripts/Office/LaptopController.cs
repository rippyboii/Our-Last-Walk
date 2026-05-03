using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Attach to the Laptop GameObject.
// Follows SafeController pattern for keypad / password validation.
public class LaptopController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject laptopCanvas;
    public GameObject lockScreenPanel;
    public GameObject emailPanel;

    [Header("Keypad")]
    public Text codeDisplay;
    public Button[] digitButtons;
    public Button clearButton;
    public Button enterButton;
    public Button closeButton;
    public GameObject wrongCodeFlash;

    [Header("References")]
    public FlashbackManager flashbackManager;
    public ProximityPrompt prompt;

    private bool playerInRange;
    private bool passwordEntered;
    private bool officeMarked;
    private string inputCode = "";

    void Start()
    {
        for (int i = 0; i < digitButtons.Length; i++)
        {
            int digit = i;
            digitButtons[i].onClick.AddListener(() => PressDigit(digit.ToString()));
        }
        clearButton?.onClick.AddListener(ClearInput);
        enterButton?.onClick.AddListener(SubmitCode);
        closeButton?.onClick.AddListener(CloseLaptop);

        if (laptopCanvas != null) laptopCanvas.SetActive(false);
        if (prompt != null && prompt.popupPanel != null) prompt.popupPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        RefreshPrompt();
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
        // E closes the laptop at any time while it is open.
        if (laptopCanvas != null && laptopCanvas.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            CloseLaptop();
            return;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (flashbackManager != null && flashbackManager.flashbackSeen)
                OpenLaptop();
        }
    }

    void OpenLaptop()
    {
        laptopCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        lockScreenPanel.SetActive(!passwordEntered);
        emailPanel.SetActive(passwordEntered);

        inputCode = "";
        UpdateDisplay();

        if (prompt != null && prompt.popupPanel != null)
            prompt.popupPanel.SetActive(false);
    }

    public void CloseLaptop()
    {
        if (passwordEntered && !officeMarked)
        {
            officeMarked = true;
            GameStateManager.Instance.officeComplete = true;
        }

        laptopCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputCode = "";
        UpdateDisplay();

        if (playerInRange) RefreshPrompt();
    }

    void PressDigit(string digit)
    {
        inputCode += digit;
        UpdateDisplay();
    }

    void ClearInput()
    {
        inputCode = "";
        UpdateDisplay();
    }

    void SubmitCode() { StartCoroutine(Validate()); }

    IEnumerator Validate()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        if (inputCode == GameStateManager.Instance.laptopPassword)
        {
            passwordEntered = true;
            lockScreenPanel.SetActive(false);
            emailPanel.SetActive(true);
        }
        else
        {
            wrongCodeFlash?.SetActive(true);
            yield return new WaitForSecondsRealtime(0.7f);
            wrongCodeFlash?.SetActive(false);
            inputCode = "";
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        if (codeDisplay != null) codeDisplay.text = inputCode;
    }

    void RefreshPrompt()
    {
        if (prompt == null || prompt.popupPanel == null) return;
        bool seen = flashbackManager != null && flashbackManager.flashbackSeen;
        prompt.popupPanel.SetActive(true);
        Text t = prompt.popupPanel.GetComponentInChildren<Text>();
        if (t != null)
            t.text = seen ? "Press E to use laptop" : "Laptop is locked";
    }
}
