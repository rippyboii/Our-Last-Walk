using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SafeController : MonoBehaviour
{
    [Header("Settings")]
    public string correctCode = "1234";

    [Header("Panels")]
    public GameObject safeCanvas;
    public GameObject keypadPanel;
    public GameObject levelCompletePanel; 

    [Header("Keypad")]
    public TMP_Text codeDisplay;
    public Button[] digitButtons;        
    public Button clearButton;
    public Button enterButton;
    public Button closeButton;
    public GameObject wrongCodeFlash;    

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
        closeButton?.onClick.AddListener(CloseSafe);
    }

    public void OpenSafe()
    {
        safeCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseSafe()
    {
        safeCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputCode = "";
        UpdateDisplay();
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

        if (inputCode == correctCode)
        {
            GameStateManager.Instance.safeUnlocked = true;
            keypadPanel.SetActive(false);
            levelCompletePanel.SetActive(true); // show win message
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
}