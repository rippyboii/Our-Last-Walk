using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public bool hasPaperCode     = false;
    public bool hasPhonePassword = false;
    public bool safeUnlocked     = false;
    public bool hasKey           = false;

    // for office laptop
    public bool officeComplete   = false;
    public string laptopPassword = "1111";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}