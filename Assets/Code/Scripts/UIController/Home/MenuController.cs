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
            LoadBoxUIData(UserProfile.Instance.SeasonIndex);
            UserProfile.Instance.IsCompleteBox = false;
        }
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");
        // Hide the menu panel and show the season panel
        _menuPanel.SetActive(false);
        _seasonPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the button click event to prevent memory leaks
        _playBtn.onClick.RemoveListener(OnPlayButtonClicked);
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    private void LoadPreviousBox(BoxData boxData)
    {
        _menuPanel.SetActive(false);
        _levelMap.gameObject.SetActive(true);
        _levelMap.LoadLevel(boxData);
    }

    private void LoadBoxUIData(int seasonIndex)
    {
        _menuPanel.SetActive(false);
        _levelMap.gameObject.SetActive(false);
        _boxSelection.gameObject.SetActive(true);
        _boxSelection.LoadBox(seasonIndex);
    }
}