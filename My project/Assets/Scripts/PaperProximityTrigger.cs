using UnityEngine;

public class PaperProximityTrigger : MonoBehaviour
{
    public GameObject promptUI;
    public PaperReadController paper;
    public float interactDistance = 1.5f;

    private Transform player;
    private bool playerInRange = false;

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null) player = playerGO.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool inRange = dist <= interactDistance;

        if (inRange != playerInRange)
        {
            playerInRange = inRange;
            if (promptUI != null) promptUI.SetActive(playerInRange);
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (promptUI != null) promptUI.SetActive(false);
            paper.OnInteract();
        }
    }
}
