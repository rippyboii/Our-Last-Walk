using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ghost;
    public GameObject dog;

    private Ghost_movement ghostMovement;
    private Dog_movement dogMovement;

    public GameObject activePlayer;

    private void Awake()
    {
        ghostMovement = ghost.GetComponent<Ghost_movement>();
        dogMovement = dog.GetComponent<Dog_movement>();
    }

    void Start()
    {
        SwitchToGhost();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // OK to keep for now
        {
            if (activePlayer == ghost)
                SwitchToDog();
            else
                SwitchToGhost();
        }
    }

    void SwitchToDog()
    {
        activePlayer = dog;

        ghostMovement.Active(false);
        dogMovement.Active(true);
    }

    void SwitchToGhost()
    {
        activePlayer = ghost;

        dogMovement.Active(false);
        ghostMovement.Active(true);
    }
}