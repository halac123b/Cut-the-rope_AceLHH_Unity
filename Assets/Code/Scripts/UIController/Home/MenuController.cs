using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _playBtn;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _seasonPanel;
    public LevelSelection LevelMap;

    private void Start()
    {
        _playBtn.onClick.AddListener(OnPlayButtonClicked);

        if (UserProfile.Instance.SelectedBoxIndex != null)
        {
            LoadPreviousBox(UserProfile.Instance.SelectedBoxIndex);

            UserProfile.Instance.SetBoxData(null);
        }
    }

    private void OnPlayButtonClicked()
    {
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
        LevelMap.gameObject.SetActive(true);
        LevelMap.LoadLevel(boxData);
    }
}