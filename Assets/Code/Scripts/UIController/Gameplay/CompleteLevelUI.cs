using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CompleteLevelUI : MonoBehaviour
{
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private Button _replayBtn;
    [SerializeField] private List<StarCollectActive> _starCollect;

    private string _levelIndex;
    private void OnEnable()
    {
        //gameObject.SetActive(false);
        _nextBtn.onClick.AddListener(OnClickNextButton);
        _menuBtn.onClick.AddListener(OnClickMenuButton);
        _replayBtn.onClick.AddListener(OnClickRestartButton);
        
        _levelIndex = UserProfile.Instance.SelectedLevelIndex;

        UpdateStarCollectLevel();
    }

    private void UpdateStarCollectLevel()
    {
        int starCollect = PlayerPrefs.GetInt($"Level_{_levelIndex}_Stars");
        
        Debug.LogError($"StarCollect: {starCollect}");

        for (int i = 0; i < starCollect; i++)
        {
            _starCollect[i].SetActiveStar(true);
        }
    }

    private void ResetUIStarCollect()
    {
        for (int i = 0; i < _starCollect.Count; i++)
        {
            _starCollect[i].SetActiveStar(false);
        }
    }

    private void OnClickMenuButton()
    {
        SceneManager.LoadScene("Home");
    }

    private void OnClickRestartButton()
    {
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.RestartLevel);
    }

    private void OnClickNextButton()
    {
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.LoadNextLevel);
    }

    private void OnDisable()
    {
        ResetUIStarCollect();
    }
}