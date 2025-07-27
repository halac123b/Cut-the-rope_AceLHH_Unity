using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _replayButton;

    public PauseUI PauseUIComponent;
    public CompleteLevelUI CompleteLevelUIComponent;

    public bool IsEnableUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _pauseButton.onClick.AddListener(() => OnPauseButtonClick());
        _replayButton.onClick.AddListener(() => OnReplaceButtonClick());

        EventDispatcher.Instance.AddEvent(gameObject, _ => ShowLevelCompleteUI(true), EventDispatcher.LoadCompleteUI);
        EventDispatcher.Instance.AddEvent(gameObject, _ => ShowLevelCompleteUI(false), EventDispatcher.DisableCompleteUI);
    }

    public void ShowLevelCompleteUI(bool active)
    {
        CompleteLevelUIComponent.gameObject.SetActive(active);
        UIStatus(active);
    }

    public void UIStatus(bool active)
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
        UIStatus(true);
    }

    private void OnReplaceButtonClick()
    {
        UIStatus(false);
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