using UnityEngine;

public class UserProfile : MonoBehaviour
{
    public static UserProfile Instance { get; private set; }

    // Example data you want to pass between scenes
    public string SelectedLevelIndex { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    // Call this before loading the game scene
    public void SetLevel(string levelIndex)
    {
        SelectedLevelIndex = levelIndex;
    }
}
