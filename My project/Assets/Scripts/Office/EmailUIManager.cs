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
    public bool starred;   // ← NEW: drives the Starred section
}

// Attach to LaptopCanvas or a child object.
// Activated by LaptopController when password is entered correctly.
public class EmailUIManager : MonoBehaviour
{
    [Header("Email Data")]
    public List<EmailEntry> emails;

    [Header("List Panel")]
    public Transform emailListContainer;
    public GameObject emailItemPrefab;

    [Header("Detail Panel")]
    public GameObject detailPanel;
    public TMP_Text senderText;
    public TMP_Text subjectText;
    public TMP_Text dateText;
    public TMP_Text bodyText;

    static readonly Color StarredHeaderBg = new Color(0.96f, 0.86f, 0.55f, 1f); // soft gold band
    static readonly Color StarredHeaderFg = new Color(0.25f, 0.18f, 0.02f, 1f);
    static readonly Color StarredRowBg    = new Color(1f,    0.985f, 0.92f, 1f); // faint cream
    static readonly Color InboxHeaderBg   = new Color(0.93f, 0.93f, 0.93f, 1f);
    static readonly Color InboxHeaderFg   = new Color(0.40f, 0.40f, 0.40f, 1f);

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

        // Partition into starred and inbox while preserving original order
        var starredIdx = new List<int>();
        var inboxIdx   = new List<int>();
        for (int i = 0; i < emails.Count; i++)
            (emails[i].starred ? starredIdx : inboxIdx).Add(i);

        if (starredIdx.Count > 0)
        {
            CreateSectionHeader("STARRED", StarredHeaderBg, StarredHeaderFg, 14f, 34f, 6f);
            foreach (var i in starredIdx) CreateEmailItem(i, true);
        }

        if (inboxIdx.Count > 0)
        {
            // Only show the INBOX divider if we also have a starred section above
            if (starredIdx.Count > 0)
                CreateSectionHeader("INBOX", InboxHeaderBg, InboxHeaderFg, 10f, 22f, 8f);
            foreach (var i in inboxIdx) CreateEmailItem(i, false);
        }
    }

    void CreateEmailItem(int i, bool isStarred)
    {
        GameObject item = Instantiate(emailItemPrefab, emailListContainer);

        TMP_Text label = item.GetComponentInChildren<TMP_Text>();
        if (label != null)
            label.text = emails[i].sender + "  —  " + emails[i].subject;

        if (isStarred)
        {
            // Tint the row background cream to distinguish from inbox rows
            Image img = item.GetComponent<Image>();
            if (img != null) img.color = StarredRowBg;
        }

        int idx = i;
        Button btn = item.GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(() => SelectEmail(idx));
    }

    void CreateSectionHeader(string label, Color bg, Color fg, float fontSize, float height, float charSpacing)
    {
        var go = new GameObject(
            label + "_Header",
            typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(LayoutElement)
        );
        go.transform.SetParent(emailListContainer, false);

        var rt = (RectTransform)go.transform;
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot     = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(0, height);

        var img = go.GetComponent<Image>();
        img.color = bg;
        img.raycastTarget = false;

        var le = go.GetComponent<LayoutElement>();
        le.preferredHeight = height;
        le.minHeight       = height;

        // Label child
        var textGO = new GameObject("Label", typeof(RectTransform));
        textGO.transform.SetParent(go.transform, false);
        var trt = (RectTransform)textGO.transform;
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.offsetMin = new Vector2(14, 0);
        trt.offsetMax = new Vector2(-14, 0);

        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text             = label;
        tmp.color            = fg;
        tmp.fontSize         = fontSize;
        tmp.fontStyle        = FontStyles.Bold;
        tmp.alignment        = TextAlignmentOptions.MidlineLeft;
        tmp.characterSpacing = charSpacing;
        tmp.enableWordWrapping = false;
        tmp.overflowMode     = TextOverflowModes.Ellipsis;
        tmp.raycastTarget    = false;
    }

    void SelectEmail(int index)
    {
        if (index < 0 || index >= emails.Count) return;
        EmailEntry e = emails[index];
        if (senderText  != null) senderText.text  = e.sender;
        if (subjectText != null) subjectText.text = e.subject;
        if (dateText    != null) dateText.text    = e.date;
        if (bodyText    != null) bodyText.text    = e.body;
        if (detailPanel != null) detailPanel.SetActive(true);
    }

    void ShowList()
    {
        if (detailPanel != null) detailPanel.SetActive(false);
    }
}