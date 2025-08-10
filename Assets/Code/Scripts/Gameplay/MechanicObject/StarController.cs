using System;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    private int _currentStarsInGameplay;
    private const int MAX_STARS = 3;
    [SerializeField] private List<Animator> _stars;
    [SerializeField] private List<Sprite> _starSprites;

    public int CurrentStarsInGameplay => _currentStarsInGameplay;

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, _ => EnableStarIndex(_currentStarsInGameplay),
            EventDispatcher.OnStarIncreased);
        EventDispatcher.Instance.AddEvent(gameObject, SetStartLevel, EventDispatcher.OnResetStars);
        EventDispatcher.Instance.AddEvent(gameObject, _ => IncreaseStars(), EventDispatcher.OnIncreaseStar);
        EventDispatcher.Instance.AddEvent(gameObject, obj =>
        {
            if (obj is Action<int> callback)
            {
                callback.Invoke(_currentStarsInGameplay);
            }
        }, EventDispatcher.OnGetStarsRequest);
        EventDispatcher.Instance.AddEvent(gameObject, _ => GetCurrentStars(), EventDispatcher.GetCurrentStar);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    private void GetCurrentStars()
    {
        UserProfile.Instance.CurrentStars = _currentStarsInGameplay;
    }

    private void SetStartLevel(object obj)
    {
        _currentStarsInGameplay = 0;
        ResetStarsUI();
        if (obj is Action<int> callback)
        {
            callback.Invoke(_currentStarsInGameplay);
        }

        EventDispatcher.Instance.Dispatch(_currentStarsInGameplay, EventDispatcher.UpdateStarNumber);
    }

    private void IncreaseStars()
    {
        if (_currentStarsInGameplay < MAX_STARS)
        {
            _currentStarsInGameplay++;
            EventDispatcher.Instance.Dispatch(_currentStarsInGameplay, EventDispatcher.OnStarIncreased);
        }

        EventDispatcher.Instance.Dispatch(_currentStarsInGameplay, EventDispatcher.UpdateStarNumber);
    }

    private void EnableStarIndex(int currentStarsInGameplay)
    {
        int starIndexInList = currentStarsInGameplay - 1;

        if (starIndexInList >= 0 && starIndexInList < _stars.Count)
        {
            Animator star = _stars[starIndexInList];
            star.speed = 0.8f;
            star.SetTrigger("StarIncrease");
        }
    }

    private void ResetStarsUI()
    {
        foreach (Animator star in _stars)
        {
            //Reset parameters in Animator
            star.Rebind();
            star.Update(0);
        }
    }
}