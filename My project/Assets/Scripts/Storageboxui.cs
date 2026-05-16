using UnityEngine;
using UnityEngine.UI;

public class StorageBoxSlideshowUI : MonoBehaviour
{
    [Header("Refs")]
    public StorageBoxController storageBox;
    public Image photoImage;
    public Text counterText;
    public Text emptyLabel;
    public Button prevButton;
    public Button nextButton;
    public Button closeButton;

    [Header("Photos (drop sprites here in Inspector)")]
    public Sprite[] photos;

    private int idx = 0;

    void Start()
    {
        prevButton?.onClick.AddListener(Prev);
        nextButton?.onClick.AddListener(Next);
        closeButton?.onClick.AddListener(Close);
    }

    void OnEnable()
    {
        idx = 0;
        Refresh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Close();
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) Prev();
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) Next();
    }

    void Prev()
    {
        if (photos == null || photos.Length == 0) return;
        idx = (idx - 1 + photos.Length) % photos.Length;
        Refresh();
    }

    void Next()
    {
        if (photos == null || photos.Length == 0) return;
        idx = (idx + 1) % photos.Length;
        Refresh();
    }

    void Close()
    {
        if (storageBox != null) storageBox.CloseSlideshow();
    }

    void Refresh()
    {
        bool hasPhotos = photos != null && photos.Length > 0;

        if (emptyLabel != null) emptyLabel.gameObject.SetActive(!hasPhotos);

        if (photoImage != null)
        {
            photoImage.gameObject.SetActive(hasPhotos);
            if (hasPhotos) photoImage.sprite = photos[idx];
        }

        if (counterText != null)
            counterText.text = hasPhotos ? string.Format("{0} / {1}", idx + 1, photos.Length) : "";

        // Hide nav buttons when there's nothing to scroll through
        bool showNav = hasPhotos && photos.Length > 1;
        if (prevButton != null) prevButton.gameObject.SetActive(showNav);
        if (nextButton != null) nextButton.gameObject.SetActive(showNav);
    }
}