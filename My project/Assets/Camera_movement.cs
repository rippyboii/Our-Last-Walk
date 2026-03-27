using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    public Transform ghost;   // the object to follow
    public Transform dog;  // the object to look at
    public Vector3 offset;     // distance from the object
    public Transform target;  // the object to look at
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = ghost; // set the target to the ghost at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (target == ghost)
            {
                target = dog; // switch to dog
            }
            else
            {
                target = ghost; // switch back to ghost
            }
        }
        Vector3 desiredPosition = target.position + offset;
        transform.position = desiredPosition;

        transform.LookAt(target);
        
    }
}
