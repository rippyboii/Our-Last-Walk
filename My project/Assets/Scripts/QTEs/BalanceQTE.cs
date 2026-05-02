using UnityEngine;

public class BalanceQTE : MonoBehaviour
{
    [Header("Needle Settings")]
    public float driftStrength = 0.3f;    // how hard the needle pulls away from center
    public float driftSpeed = 0.8f;       // how fast the drift cycles
    public float playerCorrection = 1f;   // how much mouse input affects needle
    public float needleMomentum = 0.95f;  // how slidey the needle feels (0-1, higher = more momentum)

    [Header("Fail Condition")]
    public float failThreshold = 0.85f;   // how far needle can go before failing (-1 to 1)
    public float failGracePeriod = 0.5f;  // seconds outside threshold before failing

    // the core value - everything else reads from this
    // -1 = far left, 0 = center, 1 = far right
    public float needlePosition { get; private set; }
    public event System.Action OnFail;


    private float needleVelocity;         // current momentum of the needle
    private float failTimer;              // counts up while outside threshold
    private bool isActive = false;
    private PickUp pickUp;
    private Camera_movement cameraMovement;

    void Start()
    {
        pickUp = FindObjectOfType<PickUp>();
        cameraMovement = FindObjectOfType<Camera_movement>();
        pickUp.OnFragilePickup += StartQTE;
        pickUp.OnItemDropped += StopQTE;
    }

    void Update()
    {
        // TODO: check if player is carrying something from PickUp script
        // if not carrying, make sure QTE is inactive and return

        if (!isActive) return;

        HandleDrift();
        HandlePlayerInput();
        HandleFailCondition();
        Debug.Log($"Needle Position: {needlePosition}, Velocity: {needleVelocity}, Fail Timer: {failTimer}");
    }

    void HandleDrift()
    {
        // sine wave makes the drift feel natural and cyclical
        // Time.time * driftSpeed controls how fast it cycles
        // TODO: experiment with adding perlin noise on top of sine
        // for less predictable drift
        float drift = Mathf.Sin(Time.time * driftSpeed) * driftStrength;

        // apply drift as a force to velocity, not directly to position
        // this gives it a pendulum feel rather than snapping
        needleVelocity += drift * Time.deltaTime;

        // momentum - dampen velocity each frame so it doesn't accelerate forever
        // higher value = more slidey, lower = snappier
        //needleVelocity *= needleM;

        // apply velocity to position
        needlePosition += needleVelocity * Time.deltaTime;

        // clamp so needle can't go beyond -1 and 1
        needlePosition = Mathf.Clamp(needlePosition, -1f, 1f);
    }

    void HandlePlayerInput()
    {
        // mouse delta gives us how much mouse moved this frame
        // positive = right, negative = left
        float mouseX = Input.GetAxis("Mouse X");

        // TODO: consider inverting input or scaling it differently
        // depending on feel you want
        // TODO: when you add the balancing UI, decouple mouse from camera here
        needleVelocity += mouseX * playerCorrection * Time.deltaTime;
    }

    void HandleFailCondition()
    {
        // check if needle is outside safe zone
        if (Mathf.Abs(needlePosition) > failThreshold)
        {
            failTimer += Time.deltaTime;

            // TODO: drive some visual feedback here while in danger zone
            // e.g. UI turns red, screen shakes slightly

            if (failTimer >= failGracePeriod)
            {
                TriggerFail();
            }
        }
        else
        {
            // back in safe zone - reset grace period timer
            failTimer = 0f;
        }
    }

    void TriggerFail()
    {   
        cameraMovement.isCameraLocked = false;
        isActive = false;
        needlePosition = 0f;
        needleVelocity = 0f;
        failTimer = 0f;

        // TODO: tell PickUp script to drop and break the object
        // TODO: play a sound, camera shake, visual feedback
        Debug.Log("Failed balance - object dropped");
        OnFail?.Invoke();
    }

    public void StartQTE()
    {
        cameraMovement.isCameraLocked = true;
        isActive = true;
        needlePosition = 0f;
        needleVelocity = 0f;
        failTimer = 0f;
    }

    public void StopQTE()
    {
        cameraMovement.isCameraLocked = false;
        isActive = false;
        needlePosition = 0f;
        needleVelocity = 0f;
        failTimer = 0f;
    }
}