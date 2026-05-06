using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public InputActionAsset inputActions;

    public Transform InteractionSource;
    public float InteractionRange = 100f;

    private InputAction interactAction;
    private InputActionMap playerMap;

    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player");
        interactAction = playerMap.FindAction("Interact");
    }



    private void Update()
    {
        if (interactAction.triggered) 
        {
            if (InteractionSource == null)
                InteractionSource = transform;

            Ray r = new Ray(InteractionSource.position, InteractionSource.forward);

            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractionRange))
            {
                var interactable = hitInfo.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}