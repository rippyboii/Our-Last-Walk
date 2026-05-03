using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EmailEntry
{
    public string sender;
    public string subject;
    public string date;
    [TextArea] public string body;
}

// Attach to LaptopCanvas or a child object.
// Activated by LaptopController when password is entered correctly.
// References PhoneUIManager for multi-panel navigation structure.
public class EmailUIManager : MonoBehaviour
{
    [Header("Email Data")]
    public List<EmailEntry> emails;  // fill in Inspector later

    [Header("List Panel")]
    public Transform emailListContainer;   // ScrollRect content transform
    public GameObject emailItemPrefab;     // Button prefab with a Text child

    [Header("Detail Panel")]
    public GameObject detailPanel;
    public TMP_Text senderText;
    public TMP_Text subjectText;
    public TMP_Text dateText;
    public TMP_Text bodyText;

    void OnEnable()
    {
        PopulateList();
        ShowList();
    }

    void PopulateList()
    {
        if (emailListContainer == null || emailItemPrefab == null) return;

        foreach (Transform child in emailListContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < emails.Count; i++)
        {
            int idx = i;
            GameObject item = Instantiate(emailItemPrefab, emailListContainer);
            Text label = item.GetComponentInChildren<Text>();
            if (label != null)
                label.text = $"{emails[i].sender}  —  {emails[i].subject}";
            Button btn = item.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => SelectEmail(idx));
        }
    }

    void SelectEmail(int index)
    {
        if (index < 0 || index >= emails.Count) return;
        EmailEntry e = emails[index];
        if (senderText != null)  senderText.text  = e.sender;
        if (subjectText != null) subjectText.text = e.subject;
        if (dateText != null)    dateText.text    = e.date;
        if (bodyText != null)    bodyText.text    = e.body;
        if (detailPanel != null) detailPanel.SetActive(true);
    }

    void ShowList()
    {
        if (detailPanel != null) detailPanel.SetActive(false);
    }
}
