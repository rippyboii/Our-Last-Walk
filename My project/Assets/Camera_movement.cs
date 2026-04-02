using UnityEngine;

public class Camera_movement : MonoBehaviour
{

    public Player Player; // reference to the player script
    public float mouseSensitivity = 2f; // sensitivity of mouse movement
    private float cameraVerticalRotation = 0f; // current rotation around the x-axis 

    void Start()
    {
        Cursor.visible = false; // hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor to the center of the screen
    }

    void Update()
    {
        Transform current = Player.activeplayer; // get the currently active player transform
        if (current == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        current.transform.Rotate(Vector3.up * mouseX);

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f); // limit the rotation to prevent flipping
        transform.localRotation = Quaternion.Euler(cameraVerticalRotation, current.transform.eulerAngles.y, 0);

        transform.position = current.transform.position;

    }
}
