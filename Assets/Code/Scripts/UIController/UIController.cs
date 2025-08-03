using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _replayButton;

    public PauseUI PauseUIComponent;
    public CompleteLevelUI CompleteLevelUIComponent;
    
    public bool IsEnableUI;
    public bool IsCompleteLevel;

    private void Start()
    {
        _pauseButton.onClick.AddListener(() => OnPauseButtonClick());
        _replayButton.onClick.AddListener(() => OnReplaceButtonClick());

        EventDispatcher.Instance.AddEvent(gameObject, _ => ShowLevelCompleteUI(true), EventDispatcher.LoadCompleteUI);
        EventDispatcher.Instance.AddEvent(gameObject, _ => ShowLevelCompleteUI(false), EventDispatcher.DisableCompleteUI);
    }

    public void ShowLevelCompleteUI(bool active)
    {
        IsCompleteLevel = active;
        CompleteLevelUIComponent.gameObject.SetActive(active);
        SetUIStatus(active);
    }

    public void ResetUI()
    {
        IsCompleteLevel = false;
        IsEnableUI = false;
    }

    public void SetUIStatus(bool active)
    {
        IsEnableUI = active;
    }
    
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    private void OnPauseButtonClick()
    {
        Time.timeScale = 0;
        PauseUIComponent.gameObject.SetActive(true);
        SetUIStatus(true);
    }

    private void OnReplaceButtonClick()
    {
        SetUIStatus(false);
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.RestartLevel);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            OnPauseButtonClick();
        }
    }
}