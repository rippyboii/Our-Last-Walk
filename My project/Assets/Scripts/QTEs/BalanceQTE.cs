using UnityEngine;

public class BalanceQTE : MonoBehaviour
{
    [Header("Needle Settings")]
    public float driftStrength = 0.3f;    // how hard the needle pulls away from center
    public float driftSpeed = 0.8f;       // how fast the drift cycles
    public float impulsePower = 0.075f;   // how much mouse input affects needle
    public float needleMomentum = 0.15f;  // how slidey the needle feels (0-1, higher = more momentum)

    [Header("Fail Condition")]
    public float failThreshold = 0.95f;   // how far needle can go before failing (-1 to 1)
    public float failGracePeriod = 0.5f;  // seconds outside threshold before failing

    // the core value - everything else reads from this
    // -1 = far left, 0 = center, 1 = far right
    public float needlePosition { get; private set; }
    public event System.Action OnFail;

    private float needleVelocity;         // current momentum of the needle
    private float failTimer;              // counts up while outside threshold
    private bool isActive = false;
    private PickUp pickUp;
    private BalanceUI balanceUI;
   
    void Start()
    {
        pickUp = FindObjectOfType<PickUp>();
        balanceUI = FindObjectOfType<BalanceUI>();
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
        // Debug.Log($"Needle Position: {needlePosition}, Velocity: {needleVelocity}, Fail Timer: {failTimer}");
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
        // needleVelocity *= needleMomentum;

        // apply velocity to position
        needlePosition += needleVelocity * Time.deltaTime;

        // clamp so needle can't go beyond -1 and 1
        needlePosition = Mathf.Clamp(needlePosition, -1f, 1f);
    }

    void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            needleVelocity -= impulsePower;
        } 

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            needleVelocity += impulsePower;
        }
    }

   void HandleFailCondition()
{
    float abs = Mathf.Abs(needlePosition);
    Debug.Log($"abs: {abs} threshold: {failThreshold} greater: {abs > failThreshold} timer: {failTimer}");
    
    if (abs > failThreshold)
    {
        failTimer += Time.deltaTime;
        if (failTimer >= failGracePeriod)
            TriggerFail();
    }
    else
    {
        failTimer = 0f;
    }
}

    void TriggerFail()
    {   
        balanceUI.balancePanel.SetActive(true);
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
        balanceUI.balancePanel.SetActive(true);
        isActive = true;
        needlePosition = 0f;
        needleVelocity = 0f;
        failTimer = 0f;
       
    }

    public void StopQTE()
    {
        balanceUI.balancePanel.SetActive(false);
        isActive = false;
        needlePosition = 0f;
        needleVelocity = 0f;
        failTimer = 0f;
    }
}