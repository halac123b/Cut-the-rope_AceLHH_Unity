using UnityEngine;

public class UserProfile : MonoBehaviour
{
    public static UserProfile Instance { get; private set; }

    public string SelectedLevelIndex;
    public BoxData SelectedBoxIndex;
    public int StarCollect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SelectedLevelIndex = string.Empty;
        SelectedBoxIndex = null;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLevel(string levelIndex)
    {
        SelectedLevelIndex = levelIndex;
    }

    public void SetBoxData(BoxData boxData)
    {
        SelectedBoxIndex = boxData;
    }

    public void SaveStars(string levelIndex, int stars)
    {
        int currentStars = GetStars(levelIndex);
        Debug.Log($"SaveStars called for level {levelIndex}. New stars: {stars}. Current stars: {currentStars}");

        if (stars > currentStars)
        {
            PlayerPrefs.SetInt($"Level_{levelIndex}_Stars", stars);
            StarCollect += stars;
            PlayerPrefs.SetInt("AllStars", StarCollect);
            PlayerPrefs.Save();
            Debug.Log($"Stars updated for level {levelIndex}. New stars saved: {stars}");
        }
        else
        {
            Debug.Log(
                $"No update needed for level {levelIndex}. Existing stars: {currentStars} are greater than or equal to new stars: {stars}");
        }
    }

    private int GetStars(string levelIndex)
    {
        return PlayerPrefs.GetInt($"Level_{levelIndex}_Stars", -1);
    }

    public int GetAllStars()
    {
        return PlayerPrefs.GetInt("AllStars");
    }
    
    public string GetLevel()
    {
        return SelectedLevelIndex;
    }
}