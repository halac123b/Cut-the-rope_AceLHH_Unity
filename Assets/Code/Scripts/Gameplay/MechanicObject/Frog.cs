using System;
using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour
{
    [SerializeField] private Animator _animator; 
    [SerializeField] private SpriteRenderer _frogStandPoint;
    private const float EatAnimDuration = 3.0f; 
    private int _nextLevelValue;


    private void Start()
    {
        Sprite selectedFrogSprite = UserProfile.Instance.SelectedBoxData.CharFrogSprites;
        _frogStandPoint.sprite = selectedFrogSprite;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log($"[Frog] Frog ăn Candy Object: {collision.name}");
            EventDispatcher.Instance.Dispatch(collision.gameObject, EventDispatcher.DestroyCandy);
            StartCoroutine(HandleCandyCollisionFlow(collision.gameObject));
        }
    }
    
    private IEnumerator HandleCandyCollisionFlow(GameObject candyObj)
    {
        string candyName = candyObj.name;
        
        if (_animator != null)
        {
            _animator.SetTrigger("Eat");
        }
        
        yield return new WaitForSeconds(EatAnimDuration);
        
        _animator.ResetTrigger("Eat");
        
        HandleFrogLogic(candyName);
        
        if (candyObj != null)
        {
            Destroy(candyObj);
            Debug.Log($"[Frog] Candy {candyName} đã bị destroy.");
        }
    }
    
    private void HandleFrogLogic(string candyName)
    {
        try
        {
            Debug.Log($"[Frog] Xử lý logic sau khi ăn Candy: {candyName}");

            string levelIndex = UserProfile.Instance.SelectedLevelIndex;
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.CloseLoadingCurtain);

            EventDispatcher.Instance.Dispatch(
                (Action<int>)(currentStars =>
                {
                    UserProfile.Instance.SaveStars(levelIndex, currentStars);
                    EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.LoadCompleteUI);
                    string nextLevelIndex = GetNextLevelIndex(levelIndex);

                    if (UserProfile.Instance.SelectedBoxData != null)
                    {
                        int totalLevels = UserProfile.Instance.SelectedBoxData.NumberOfLevels;

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
                        Debug.LogWarning("[Frog] SelectedBoxData is null.");
                    }
                }),
                EventDispatcher.OnGetStarsRequest
            );
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Frog] Lỗi xử lý Candy {candyName}: {ex.Message}");
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