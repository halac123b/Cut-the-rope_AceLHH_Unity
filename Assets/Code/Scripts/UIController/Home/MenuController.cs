using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _playBtn;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _seasonPanel;
    [SerializeField] private LevelSelection _levelMap;
    [SerializeField] private BoxSelection _boxSelection;

    private void Start()
    {
        _playBtn.onClick.AddListener(OnPlayButtonClicked);

        if (UserProfile.Instance.SelectedBoxData != null && !UserProfile.Instance.IsCompleteBox)
        {
            LoadPreviousBox(UserProfile.Instance.SelectedBoxData);
        }
        
        if (UserProfile.Instance.SeasonIndex != -1 && UserProfile.Instance.IsCompleteBox)
        {
            LoadBoxUIData();
            UserProfile.Instance.IsCompleteBox = false;
        }
        else
        {
             _menuPanel.SetActive(true);
        }
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");

        Transition.Instance.Appear(Color.black, () =>
        {
            _menuPanel.SetActive(false);
            _seasonPanel.SetActive(true);
        });
    }

    private void OnDestroy()
    {
        // Unsubscribe from the button click event to prevent memory leaks
        _playBtn.onClick.RemoveListener(OnPlayButtonClicked);
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    private void LoadPreviousBox(BoxData boxData)
    {
        Transition.Instance.Appear(Color.black, () =>
        {
            _menuPanel.SetActive(false);
            _levelMap.gameObject.SetActive(true);
        });

        _levelMap.LoadLevel(boxData);
    }

    private void LoadBoxUIData()
    {
        _boxSelection.gameObject.SetActive(true);
    }
}