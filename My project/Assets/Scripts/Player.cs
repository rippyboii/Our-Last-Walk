using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public GameObject ghost;
    public GameObject dog;
    public GameObject bond;

    private Ghost_movement ghostMovement;
    private Dog_movement dogMovement;

    public GameObject activePlayer;

    public Volume blurVolume;
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
        blurVolume.GetComponent<VisionBlur>().SetBlurry(true);
        activePlayer = dog;
        bond.SetActive(false);
        ghost.SetActive(false);
        ghostMovement.Active(false);
        
        dogMovement.Active(true);
    }

    void SwitchToGhost()
    {
        blurVolume.GetComponent<VisionBlur>().SetBlurry(false);
        activePlayer = ghost;
        bond.SetActive(true);
        ghost.transform.position = dog.transform.position + new Vector3(0, 0.5f, 0);
        ghost.SetActive(true);
        dogMovement.Active(false);
        ghostMovement.Active(true);
    }
}