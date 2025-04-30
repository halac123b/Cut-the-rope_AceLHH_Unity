using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    private static int _currentStarsInGameplay;
    private const int MAX_STARS = 3;
    private static StarController Instance { get; set; }
    [SerializeField] private List<Animator> _stars;

    private void Awake()
    {
        Instance = this;
    }

    public static int SetStartLevel()
    {
        Debug.Log("[Kiên],[Star] Reset stars to 0");
        _currentStarsInGameplay = 0;
        return _currentStarsInGameplay;
    }

    public static void IncreaseStars()
    {
        if (_currentStarsInGameplay < MAX_STARS)
        {
            _currentStarsInGameplay++;
            Instance?.EnableStarIndex(_currentStarsInGameplay);
            Debug.Log($"[Kien],[Star] Stars increased: {_currentStarsInGameplay}");
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