using System;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    private static int _currentStarsInGameplay;
    private const int MAX_STARS = 3;
    [SerializeField] private List<Animator> _stars;

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, _ => EnableStarIndex(_currentStarsInGameplay), EventDispatcher.OnStarIncreased);
    }

    public static int SetStartLevel()
    {
        _currentStarsInGameplay = 0;
        return _currentStarsInGameplay;
    }

    public static void IncreaseStars()
    {
        if (_currentStarsInGameplay < MAX_STARS)
        {
            _currentStarsInGameplay++;
            EventDispatcher.Instance.Dispatch(_currentStarsInGameplay, EventDispatcher.OnStarIncreased);
        }

        EventDispatcher.Instance.Dispatch(_currentStarsInGameplay, EventDispatcher.UpdateStarNumber);
    }

    public static int GetStarsInGameplay()
    {
        return _currentStarsInGameplay;
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
}