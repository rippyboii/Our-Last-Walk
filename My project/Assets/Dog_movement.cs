using UnityEngine;

public class Dog_movement : MonoBehaviour
{
        private Camera_movement camera_movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera_movement = Camera.main.GetComponent<Camera_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camera_movement.player == transform){
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * 5);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * Time.deltaTime * 5);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * 5);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * 5);
            }
            if (Input.GetKey(KeyCode.Space) )
            {
                transform.Translate(Vector3.up * Time.deltaTime * 5);
            }
        }
    }
}
