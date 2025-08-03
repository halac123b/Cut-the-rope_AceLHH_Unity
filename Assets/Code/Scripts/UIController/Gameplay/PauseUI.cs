using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _resumeBtn;

    private void Start()
    {
        _backBtn.onClick.AddListener(OnBackClick);
        _resumeBtn.onClick.AddListener(OnResumeClick);
    }

    private void OnResumeClick()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        UIController.Instance.SetUIStatus(false);
    }

    private void OnBackClick()
    {
        string levelIndex = UserProfile.Instance.SelectedLevelIndex;
        EventDispatcher.Instance.Dispatch(
            (Action<int>)(_ => { UserProfile.Instance.SaveStars(levelIndex, 0); }),
            EventDispatcher.OnGetStarsRequest
        );

        UIController.Instance.SetUIStatus(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
    }
}