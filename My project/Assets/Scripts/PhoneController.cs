using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject phoneCanvas;

    public void OnInteract()
    {
        phoneCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePhone()
    {
        phoneCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}