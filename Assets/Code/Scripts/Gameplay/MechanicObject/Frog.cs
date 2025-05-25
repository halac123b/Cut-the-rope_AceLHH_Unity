using System;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private int _nextLevelValue;
    public static event Action<Collider2D> OnCandyCollision;

    private void OnEnable()
    {
        OnCandyCollision += HandleFrogCollision;
    }

    private void OnDisable()
    {
        OnCandyCollision -= HandleFrogCollision;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log($"[Frog] Frog ăn Candy Object: {collision.name}");
            OnCandyCollision?.Invoke(collision);
            Destroy(collision.gameObject, 0.015f);
        }
    }

    private void HandleFrogCollision(Collider2D collision)
    {
        try
        {
            Debug.Log($"[Frog] Xử lý va chạm, Frog ăn Candy Object: {collision.name}");
            
            string levelIndex = UserProfile.Instance.SelectedLevelIndex;
            
            EventDispatcher.Instance.Dispatch(
                (Action<int>)(currentStars => {
                   
                    UserProfile.Instance.SaveStars(levelIndex, currentStars);
                    EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.LoadCompleteUI);
                    string nextLevelIndex = GetNextLevelIndex(levelIndex);

                    if (UserProfile.Instance.SelectedBoxIndex != null)
                    {
                        int totalLevels = UserProfile.Instance.SelectedBoxIndex.NumberOfLevels;

                        if (_nextLevelValue <= totalLevels)
                        {
                            EventDispatcher.Instance.Dispatch(
                                (Action<int>)(resetStars =>
                                {
                                    UserProfile.Instance.SaveStars(nextLevelIndex, resetStars);
                                }),
                                EventDispatcher.OnResetStars
                            );
                        }
                    }
                    else
                    {
                        Debug.LogWarning("[Frog] SelectedBoxIndex is null.");
                    }
                }),
                EventDispatcher.OnGetStarsRequest
            );
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Frog] Lỗi khi Frog ăn Candy Object: {collision.name} - {ex.Message}");
        }
    }
    
    private string GetNextLevelIndex(string currentLevelIndex)
    {
        string[] parts = currentLevelIndex.Split('_');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int boxIndex) &&
            int.TryParse(parts[1], out int levelNumber))
        {
            levelNumber += 1;
            _nextLevelValue = levelNumber;
            return $"{boxIndex}_{levelNumber}";
        }

        return currentLevelIndex;
    }
}