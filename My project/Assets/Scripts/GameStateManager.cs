using UnityEngine;


public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool safe1Unlocked = false;
    public bool doorUnlocked = false;
    public bool computerUnlocked = false;
    public bool photosSolved = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
