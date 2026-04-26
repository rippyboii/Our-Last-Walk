using NUnit.Framework;
using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private LayerMask pickableLayer;
    [SerializeField]
    private Transform playerCameraTransform;
    [SerializeField]
    private GameObject pickUpUI;
    [SerializeField]
    private float hitRange = 3;
    [SerializeField]
    private Transform pickupParent;

    private GameObject currentlyCarried;

    [SerializeField]
    private  InputActionReference interactionInput, dropInput;
    private RaycastHit hit;
    private Player player;

   void Start()
    {
         player = FindObjectOfType<Player>();
         interactionInput.action.performed += Interact;
         dropInput.action.performed += Drop;

    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (hit.collider == null) return;
        Rigidbody rb= hit.collider.GetComponent<Rigidbody>();

        if (hit.collider.GetComponent<Item>())
        {
            Debug.Log("Picking up " + hit.collider.name);
            currentlyCarried = hit.collider.gameObject;
            //rrentlyCarried.transform.position = Vector3.zero;
            currentlyCarried.transform.rotation = Quaternion.identity;
            currentlyCarried.transform.SetParent(pickupParent.transform, true);
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

        private void Drop(InputAction.CallbackContext context)
    {
        if (currentlyCarried == null) return;
        Debug.Log("Dropping " + currentlyCarried.name);
        currentlyCarried.transform.SetParent(null, true);
        Rigidbody rb = currentlyCarried.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        currentlyCarried = null;
    }

    
    // Update is called once per frame
    void Update()
    {   
        if (!player.IsDog())
        {
            pickUpUI.SetActive(false);
            return;
        } 
        // don't do anything if we're already carrying something
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }
        
        if (currentlyCarried != null) return;
        if (Physics.Raycast(
            playerCameraTransform.position,
            playerCameraTransform.forward,
            out hit,
            hitRange,
            pickableLayer))
        {
           
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.SetActive(true);

        }
    }
}
