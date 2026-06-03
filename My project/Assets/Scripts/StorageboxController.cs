using UnityEngine;

public class StorageBoxController : MonoBehaviour
{
    public GameObject slideshowCanvas;

    public void OpenSlideshow()
    {
        if (slideshowCanvas != null) slideshowCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseSlideshow()
    {
        if (slideshowCanvas != null) slideshowCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}