using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    public Transform ghost;   // the object to follow
    public Transform dog;  // the object to look at
    public Transform player;  // the object to look at


    public float mouseSensitivity = 2f; // sensitivity of mouse movement
    private float cameraVerticalRotation = 0f; // current rotation around the x-axis 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = ghost;
        Cursor.visible = false; // hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor to the center of the screen
 
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        player.Rotate(Vector3.up * mouseX); 


        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f); // limit the rotation to prevent flipping
        transform.localRotation = Quaternion.Euler(cameraVerticalRotation, player.eulerAngles.y, 0);


        transform.position = player.position;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (player == ghost)
            {
                player = dog;
            }
            else
            {
                player = ghost;
            }
        }


    }
}
