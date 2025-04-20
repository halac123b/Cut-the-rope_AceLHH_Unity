using System;
using Unity.VisualScripting;
using UnityEngine;

public class StarController : MonoBehaviour
{
    private static int _currentStarsInGameplay;
    private const int MAX_STARS = 3;

    public void ResetStars()
    {
        _currentStarsInGameplay = 0;
        Debug.Log("[Kiên],[Star] Reset stars to 0");
    }
    
    public static void IncreaseStars()
    {
        if (_currentStarsInGameplay < MAX_STARS)
        {
            _currentStarsInGameplay++;
            Debug.Log($"[Kien],[Star] Stars increased: {_currentStarsInGameplay}");
        }
        
        EventDispatcher.Instance.Dispatch(_currentStarsInGameplay, EventDispatcher.UpdateStarNumber );
    }
    
    public static int GetStarsInGameplay()
    {
        return _currentStarsInGameplay;
    }
}

