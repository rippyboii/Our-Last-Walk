using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;


    public Transform activeplayer;
    public Transform ghost;
    public Transform dog;

    void Start()
    {
        activeplayer = ghost; // start as ghost
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (activeplayer == ghost)
                activeplayer = dog;
            else
                activeplayer = ghost;
        }
    }
}
