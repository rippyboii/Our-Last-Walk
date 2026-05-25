using UnityEngine;

public class Drawer : MonoBehaviour
{
    public float openDistance = 0.4f;
    public float speed = 2f;
    public int axis = 2; // 0 = x, 1 = y, 2 = z
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    void Start()
    {
        closedPosition = transform.localPosition;
        if (axis == 0)
            openPosition = closedPosition + new Vector3(openDistance, 0, 0);
        else if (axis == 1)
            openPosition = closedPosition + new Vector3(0, openDistance, 0);
        else
        openPosition = closedPosition + new Vector3(0, 0, openDistance); // adjust axis
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPosition : closedPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
    }
}