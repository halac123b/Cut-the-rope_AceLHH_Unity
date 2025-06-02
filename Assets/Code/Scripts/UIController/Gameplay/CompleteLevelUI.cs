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
    [SerializeField] private List<StarCollectActive> _starCollect;

    private string _levelIndex;
    private void OnEnable()
    {
        //gameObject.SetActive(false);
        _menuBtn.onClick.AddListener(OnClickMenuButton);
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
        
    }

    private void OnClickNextButton()
    {
        
    }

    private void OnDisable()
    {
        ResetUIStarCollect();
    }
}