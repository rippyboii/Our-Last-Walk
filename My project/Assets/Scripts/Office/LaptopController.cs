using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LaptopController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject laptopCanvas;
    public GameObject lockScreenPanel;
    public GameObject emailPanel;

    [Header("Password Input")]
    public InputField passwordInput;
    public GameObject wrongCodeFlash;
    public Button closeButton;

    [Header("Prompt")]
    public GameObject promptUI;

    [Header("References")]
    public FlashbackManager flashbackManager;

    private bool playerInRange;
    private bool passwordEntered;
    private bool officeMarked;

    void Start()
    {
        if (laptopCanvas != null) laptopCanvas.SetActive(false);
        if (promptUI != null) promptUI.SetActive(false);
        closeButton?.onClick.AddListener(CloseLaptop);
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
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        if (laptopCanvas != null && laptopCanvas.activeSelf)
        {
            // E closes at any time
            if (Input.GetKeyDown(KeyCode.E))
            {
                CloseLaptop();
                return;
            }

            // Enter submits the password
            if (lockScreenPanel != null && lockScreenPanel.activeSelf
                && Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Validate());
            }
        }

        if (playerInRange && !laptopCanvas.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            bool seen = flashbackManager == null || flashbackManager.flashbackSeen;
            if (seen) OpenLaptop();
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

        if (passwordInput != null)
        {
            passwordInput.text = "";
            passwordInput.ActivateInputField();
        }

        if (promptUI != null) promptUI.SetActive(false);
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

        if (passwordInput != null) passwordInput.text = "";
        if (playerInRange) RefreshPrompt();
    }

    IEnumerator Validate()
    {
        string entered = passwordInput != null ? passwordInput.text : "";

        yield return new WaitForSecondsRealtime(0.15f);

        if (entered == GameStateManager.Instance.laptopPassword)
        {
            passwordEntered = true;
            lockScreenPanel.SetActive(false);
            emailPanel.SetActive(true);
        }
        else
        {
            if (wrongCodeFlash != null) wrongCodeFlash.SetActive(true);
            yield return new WaitForSecondsRealtime(0.7f);
            if (wrongCodeFlash != null) wrongCodeFlash.SetActive(false);
            if (passwordInput != null)
            {
                passwordInput.text = "";
                passwordInput.ActivateInputField();
            }
        }
    }

    void RefreshPrompt()
    {
        if (promptUI == null) return;
        bool seen = flashbackManager == null || flashbackManager.flashbackSeen;
        promptUI.SetActive(true);
        Text t = promptUI.GetComponentInChildren<Text>();
        if (t != null)
            t.text = seen ? "Press E to use laptop" : "Laptop is locked";
    }
}
