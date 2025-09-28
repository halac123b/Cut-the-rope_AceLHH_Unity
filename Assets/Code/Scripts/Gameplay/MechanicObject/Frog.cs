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
        
        EventDispatcher.Instance.AddEvent(gameObject, _ => PlayAnimationLevelFail(), EventDispatcher.LevelFail);
        EventDispatcher.Instance.AddEvent(gameObject, _ => PlayAnimationCollectStar(), EventDispatcher.CollectStar);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log($"[Frog] Frog ăn Candy Object: {collision.name}");
            // EventDispatcher.Instance.Dispatch(collision.gameObject, EventDispatcher.DestroyCandy);
            // collision.gameObject.SetActive(false);
            
            Candy candy = collision.gameObject.GetComponent<Candy>();
            candy.FadeAllRopes();
            
            float height = GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 frogPos = transform.position + new Vector3(0, height / 2f, 0);
            collision.gameObject.GetComponent<Candy>().BeEaten(frogPos);
            
            StartCoroutine(HandleCandyCollisionFlow(collision.gameObject));
        }
    }

    private void PlayAnimationLevelFail()
    {
        _animator.Play("Sad");
    }
    
    private void PlayAnimationCollectStar()
    {
        _animator.Play("Forward");
    }

    private void LevelFailHandle()
    {
        EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.RestartLevel);
    }

    private IEnumerator HandleCandyCollisionFlow(GameObject candyObj)
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Eat");
        }

        yield return new WaitForSeconds(EatAnimDuration);
        
        UIController.Instance.IsCompleteLevel = true;
        
        _animator.ResetTrigger("Eat");

        HandleFrogLogic();

        if (candyObj != null)
        {
            // Destroy(candyObj);
            Debug.Log($"[Frog] Candy đã bị destroy.");
        }
    }

    private void HandleFrogLogic()
    {
        Debug.Log($"[Frog] Xử lý logic sau khi ăn Candy");
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

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}