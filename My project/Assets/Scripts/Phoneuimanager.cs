using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUIManager : MonoBehaviour
{
    [Header("PIN Settings")]
    public string correctPin = "7777";

    [Header("Phone Controller")]
    public PhoneController phoneController;

    [Header("Exit Hint")]
    public GameObject exitHint;

    [Header("Screens")]
    public GameObject lockScreen;
    public GameObject homeScreen;
    public GameObject messagesPanel;
    public GameObject blockedPanel;
    public GameObject exWifeThread;

    [Header("PIN Display")]
    public Text pinDisplay;
    public Text errorLabel;

    [Header("Numpad Buttons (0-9 in order, then Delete)")]
    public Button[] numpadButtons;
    public Button deleteButton;

    [Header("Navigation Buttons")]
    public Button messagesAppBtn;
    public Button blockedContactsBtn;
    public Button exWifeRowBtn;
    public Button backFromMessages;
    public Button backFromBlocked;
    public Button backFromExWife;
    public Button homeButtonClose;

    private string enteredPin = "";
    private bool exWifeOpened = false;

    void OnEnable()
    {
        ShowOnly(lockScreen);
        enteredPin = "";
        UpdatePinDisplay();
        if (errorLabel != null) errorLabel.gameObject.SetActive(false);
        if (exitHint != null) exitHint.SetActive(true);
    }

    void OnDisable()
    {
        if (exitHint != null) exitHint.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ClosePhone();
    }

    void Start()
    {
        for (int i = 0; i < numpadButtons.Length; i++)
        {
            int digit = i;
            numpadButtons[i].onClick.AddListener(() => PressDigit(digit.ToString()));
        }

        if (deleteButton != null)
            deleteButton.onClick.AddListener(DeleteDigit);

        if (messagesAppBtn != null)
            messagesAppBtn.onClick.AddListener(() => ShowOnly(messagesPanel));

        if (blockedContactsBtn != null)
            blockedContactsBtn.onClick.AddListener(() => ShowOnly(blockedPanel));

        if (exWifeRowBtn != null)
            exWifeRowBtn.onClick.AddListener(OpenExWifeThread);

        if (backFromMessages != null)
            backFromMessages.onClick.AddListener(() => ShowOnly(homeScreen));

        if (backFromBlocked != null)
            backFromBlocked.onClick.AddListener(() => ShowOnly(messagesPanel));

        if (backFromExWife != null)
            backFromExWife.onClick.AddListener(() => ShowOnly(blockedPanel));

        if (homeButtonClose != null)
            homeButtonClose.onClick.AddListener(ClosePhone);
    }

    public void ClosePhone()
    {
        if (exitHint != null) exitHint.SetActive(false);
        if (phoneController != null) phoneController.ClosePhone();
    }

    public void PressDigit(string digit)
    {
        if (enteredPin.Length >= 4) return;
        enteredPin += digit;
        UpdatePinDisplay();
        if (enteredPin.Length == 4) StartCoroutine(ValidatePin());
    }

    public void DeleteDigit()
    {
        if (enteredPin.Length == 0) return;
        enteredPin = enteredPin.Substring(0, enteredPin.Length - 1);
        UpdatePinDisplay();
    }

    void UpdatePinDisplay()
    {
        if (pinDisplay == null) return;
        string display = "";
        for (int i = 0; i < 4; i++)
            display += (i < enteredPin.Length ? "●" : "_") + (i < 3 ? "  " : "");
        pinDisplay.text = display;
    }

    IEnumerator ValidatePin()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        if (enteredPin == correctPin)
        {
            enteredPin = "";
            UpdatePinDisplay();
            ShowOnly(homeScreen);
        }
        else
        {
            if (pinDisplay != null) pinDisplay.color = new Color(0.90f, 0.20f, 0.20f, 1f);
            if (errorLabel != null) errorLabel.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.7f);
            if (pinDisplay != null) pinDisplay.color = Color.white;
            if (errorLabel != null) errorLabel.gameObject.SetActive(false);
            enteredPin = "";
            UpdatePinDisplay();
        }
    }

    void ShowOnly(GameObject target)
    {
        lockScreen?.SetActive(false);
        homeScreen?.SetActive(false);
        messagesPanel?.SetActive(false);
        blockedPanel?.SetActive(false);
        exWifeThread?.SetActive(false);
        if (target != null) target.SetActive(true);
    }

    void OpenExWifeThread()
    {
        ShowOnly(exWifeThread);
        if (!exWifeOpened)
        {
            exWifeOpened = true;
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.hasPhonePassword = true;
            Debug.Log("GameStateManager.hasPhonePassword = true");
        }
    }
}