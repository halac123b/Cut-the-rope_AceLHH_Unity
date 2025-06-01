using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _replayButton;

    public PauseUI PauseUIComponent;
    // public StarUI StarUIComponent;
    public CompleteLevelUI CompleteLevelUIComponent;

    private void Start()
    {
        _pauseButton.onClick.AddListener(() => OnPauseButtonClick());
        _replayButton.onClick.AddListener(() => OnReplaceButtonClick());

        EventDispatcher.Instance.AddEvent(gameObject, _ => ShowLevelCompleteUI(),
            EventDispatcher.LoadCompleteUI);
    }

    public void ShowLevelCompleteUI()
    {
        //StarUIComponent.gameObject.SetActive(false);
        CompleteLevelUIComponent.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    private void OnPauseButtonClick()
    {
        Time.timeScale = 0;
        PauseUIComponent.gameObject.SetActive(true);
    }

    private void OnReplaceButtonClick()
    {
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.RestartLevel);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("OnApplicationFocus: " + hasFocus);
        if (hasFocus)
        {
            OnPauseButtonClick();
        }
        // else
        // {
        //     Time.timeScale = 0;
        // }
    }
}