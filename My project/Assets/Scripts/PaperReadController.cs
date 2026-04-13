using UnityEngine;

public class PaperReadController : MonoBehaviour
{
    public GameObject paperCanvas;

    void Update()
    {
        if (paperCanvas != null && paperCanvas.activeSelf && Input.GetKeyDown(KeyCode.E))
            ClosePaper();
    }

    public void OnInteract()
    {
        paperCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePaper()
    {
        paperCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
