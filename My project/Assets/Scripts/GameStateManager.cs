using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public bool hasPaperCode      = false;
    public bool hasPhonePassword  = false;
    public bool safeUnlocked      = false;
    public bool hasKey            = false;

    private void Awake()
    {
        // Enforce singleton, destroy duplicates, persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}