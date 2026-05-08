using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ghost;
    public GameObject dog;
    public GameObject bond;

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
        bond.SetActive(false);
        ghost.SetActive(false);
        ghostMovement.Active(false);
        
        dogMovement.Active(true);
    }

    void SwitchToGhost()
    {
        activePlayer = ghost;
        bond.SetActive(true);
        ghost.transform.position = dog.transform.position + new Vector3(0, 0.5f, 0);
        ghost.SetActive(true);
        dogMovement.Active(false);
        ghostMovement.Active(true);
    }

    public bool IsDog()
{
    return activePlayer == dog;
}
}