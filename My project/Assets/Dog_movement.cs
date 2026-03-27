using UnityEngine;

public class Dog_movement :  MonoBehaviour
{
    public float speed = 5f;
    public Player player;
    void Start()
    {

    }

    void Update()
    {
        if(player.activeplayer == this.transform){  
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.back * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.A))
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.Space))
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.LeftShift))
                transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
    }


}
