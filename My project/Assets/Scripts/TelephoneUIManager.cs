using UnityEngine;
using UnityEngine.UI;

public class TelephoneUIManager : MonoBehaviour
{
    [Header("Telephone Controller")]
    public TelephoneController telephoneController;

    [Header("Canvas")]
    public GameObject telephoneCanvas;

    [Header("Exit Hint")]
    public GameObject exitHint;

    [Header("Panels")]
    public GameObject contactsPanel;
    public GameObject nowPlayingPanel;

    [Header("Contacts (5 buttons in order)")]
    public Button[] contactButtons;
    public string[] contactNames = { "Mom", "Dad", "Alex", "Sam", "Jordan" };

    [Header("Now Playing")]
    public Text nowPlayingLabel;
    public Button stopButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] tracks;          // reuse turntable tracks for now

    void Start()
    {
        for (int i = 0; i < contactButtons.Length; i++)
        {
            int index = i;
            contactButtons[i].onClick.AddListener(() => PlayTrack(index));
        }

        if (stopButton != null)
            stopButton.onClick.AddListener(StopTrack);
    }

    public void OnEnterMode()
    {
        if (telephoneCanvas != null) telephoneCanvas.SetActive(true);
        if (exitHint != null)        exitHint.SetActive(true);
        ShowOnly(contactsPanel);
    }

    public void OnExitMode()
    {
        audioSource?.Stop();
        if (telephoneCanvas != null) telephoneCanvas.SetActive(false);
        if (exitHint != null)        exitHint.SetActive(false);
    }

    public void ShowContacts()
    {
        ShowOnly(contactsPanel);
    }

    void PlayTrack(int index)
    {
        if (audioSource == null || tracks == null || index >= tracks.Length) return;

        audioSource.clip = tracks[index];
        audioSource.Play();

        if (nowPlayingLabel != null)
            nowPlayingLabel.text = "Playing: " + 
                (index < contactNames.Length ? contactNames[index] : "Unknown");

        ShowOnly(nowPlayingPanel);
    }

    void StopTrack()
    {
        audioSource?.Stop();
        ShowOnly(contactsPanel);
    }

    void ShowOnly(GameObject target)
    {
        contactsPanel?.SetActive(false);
        nowPlayingPanel?.SetActive(false);
        if (target != null) target.SetActive(true);
    }
}