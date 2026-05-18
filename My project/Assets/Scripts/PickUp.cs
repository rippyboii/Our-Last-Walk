using NUnit.Framework;
using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public event System.Action OnFragilePickup;
    public event System.Action OnItemDropped;
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

    public GameObject currentlyCarried;
    public Transform fragile_parent;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private BalanceQTE balanceQTE;
    private Item item;

    [SerializeField] 
    private LayerMask lampLayer;
    private LampHighlighter currentLamp;
   


    [SerializeField]
    private  InputActionReference interactionInput, dropInput;
    private RaycastHit hit;
    private RaycastHit ghostHit;  // new ghost hit

    private Player player;

   void Start()
    {
        balanceQTE = FindObjectOfType<BalanceQTE>();
        balanceQTE.OnFail += HandleFragileFail;
        player = FindObjectOfType<Player>();
        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;

    }
    public void ResetLampReference()
    {
        currentLamp = null;
    }
    private void Interact(InputAction.CallbackContext context)
    {
        if (player.IsDog())
        {
            if (hit.collider == null) return;
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            ParticleSystem ps = hit.collider.GetComponent<ParticleSystem>();

            if (hit.collider.GetComponent<Item>())
            {
                item = hit.collider.GetComponent<Item>();
                if (item.isFragile)
                {
                    OnFragilePickup?.Invoke();
                }
                Debug.Log("Picking up " + hit.collider.name);
                currentlyCarried = hit.collider.gameObject;
                //rrentlyCarried.transform.position = Vector3.zero;
                initialPosition = currentlyCarried.transform.position;
                initialRotation = currentlyCarried.transform.rotation;
                currentlyCarried.transform.rotation = Quaternion.identity;
                currentlyCarried.transform.SetParent(pickupParent.transform, true);
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }
                if (ps != null)
                {
                    ps.Stop();
                }
            }
        }
        else
        {
            if (ghostHit.collider == null) return;
            LampHighlighter lamp = ghostHit.collider.GetComponent<LampHighlighter>();
            if (lamp != null)
            {
                if (currentLamp != null && currentLamp != lamp)
                {
                    currentLamp.Deactivate();
                    Debug.Log("Deactivating previous lamp");
                }
                    
                    
                lamp.Activate();
                currentLamp = lamp;
                player.currentActiveLamp = lamp;
            }
        }



    }

    private void Drop(InputAction.CallbackContext context)
    {
        if (currentlyCarried == null) return;
        Debug.Log("Dropping " + currentlyCarried.name);
        currentlyCarried.transform.SetParent(null, true);
        Rigidbody rb = currentlyCarried.GetComponent<Rigidbody>();
        ParticleSystem ps = currentlyCarried.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        currentlyCarried = null;
        OnItemDropped?.Invoke();
    }
    void HandleFragileFail()
    {
        if (currentlyCarried == null) return;
        
        // TODO: play break sound, spawn broken prefab, particles etc
        currentlyCarried.transform.SetParent(fragile_parent);
        currentlyCarried.transform.position = initialPosition;
        currentlyCarried.transform.rotation = initialRotation;
        Rigidbody rb = currentlyCarried.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        currentlyCarried = null;
    }
    
    // Update is called once per frame
    void Update()
    {   
        if (!player.IsDog())
        {
            pickUpUI.SetActive(false);
            if (ghostHit.collider != null)
            {
            ghostHit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
            }
        
            if (Physics.Raycast(
            playerCameraTransform.position,
            playerCameraTransform.forward,
            out ghostHit,
            hitRange,
            lampLayer))
            {
                ghostHit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                Debug.Log("Looking at " + ghostHit.collider.name);
            }
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
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
