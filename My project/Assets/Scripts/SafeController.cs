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

    public bool isFinalSafe = false; // set to true for the door

    [Header("Door Animation")]
    public Transform safeDoor;
    public Transform hingePoint;        // empty GameObject at hinge edge
    public float doorOpenAngle = 120f;  // total degrees to swing open
    public float doorOpenSpeed = 60f;   // degrees per second
    public GameObject lastDrawing;


    private bool doorIsOpening = false;
    private float angleTurned = 0f;

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

    // IEnumerator Validate()
    // {
    //     yield return new WaitForSecondsRealtime(0.15f);

    //     if (inputCode == correctCode)
    //     {
    //         GameStateManager.Instance.safeUnlocked = true;
    //         keypadPanel.SetActive(false);
    //         levelCompletePanel.SetActive(true); // show win message
    //     }
    //     else
    //     {
    //         wrongCodeFlash?.SetActive(true);
    //         yield return new WaitForSecondsRealtime(0.7f);
    //         wrongCodeFlash?.SetActive(false);
    //         inputCode = "";
    //         UpdateDisplay();
    //     }
    // }

    IEnumerator Validate()
{
    Debug.Log("Validate started");
    yield return new WaitForSecondsRealtime(0.15f);
    Debug.Log("After wait - inputCode: '" + inputCode + "' correctCode: '" + correctCode + "' match: " + (inputCode.Trim() == correctCode.Trim()));

    if (inputCode.Trim() == correctCode.Trim())
    {
        if (isFinalSafe)
        {
            GameStateManager.Instance.doorUnlocked = true;
            FindObjectOfType<SceneTransitionManager>().TriggerTransition();
            yield break; 
        }
    else
        GameStateManager.Instance.safe1Unlocked = true;
        keypadPanel.SetActive(false);
        levelCompletePanel.SetActive(true);
        CloseSafe();
        
        // open door and reveal note
        doorIsOpening = true;
        yield return new WaitForSecondsRealtime(1f); // wait for door to swing open
        lastDrawing.SetActive(true); // reveal the note

    }
    else
    {
        Debug.Log("WRONG");
    }
}

    void UpdateDisplay()
    {
        if (codeDisplay != null) codeDisplay.text = inputCode;
        Debug.Log("Current Input: " + inputCode);
        Debug.Log("Correct Code: " + correctCode);
    }

   void Update()
{
    // existing update code...

    if (doorIsOpening && Mathf.Abs(angleTurned) < Mathf.Abs(doorOpenAngle))
    {
        float step = doorOpenSpeed * Time.deltaTime;
        
        // rotate in correct direction based on sign of doorOpenAngle
        float direction = Mathf.Sign(doorOpenAngle);
        safeDoor.RotateAround(hingePoint.position, Vector3.up, step * direction);
        angleTurned += step;

        if (angleTurned >= Mathf.Abs(doorOpenAngle))
        {
            doorIsOpening = false;
            if (lastDrawing != null)
                lastDrawing.SetActive(true);
        }
    }
}
}