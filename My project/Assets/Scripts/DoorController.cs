using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    public Transform hingePoint;  // Assign the DoorHinge empty GameObject here
    public GameObject promptUI;

    [Header("Door Settings")]
    public float openAngle = 90f;       
    public float speed = 90f;

    private bool dogInRange = false;
    private bool isOpen = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            dogInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            dogInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (dogInRange && Input.GetKeyDown(KeyCode.Alpha9))
        {
            isOpen = !isOpen;
            targetAngle = isOpen ? openAngle : 0f;
        }

        if (Mathf.Abs(currentAngle - targetAngle) > 0.01f)
        {
            Vector3 hingePos = hingePoint != null ? hingePoint.position : transform.position;
            float step = speed * Time.deltaTime * (targetAngle > currentAngle ? 1f : -1f);
            float remaining = targetAngle - currentAngle;
            if (Mathf.Abs(step) > Mathf.Abs(remaining)) step = remaining;

            transform.RotateAround(hingePos, Vector3.up, step);
            currentAngle += step;
        }
    }
}