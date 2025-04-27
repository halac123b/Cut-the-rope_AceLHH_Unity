using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _playBtn;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _seasonPanel;
    [SerializeField] private LevelSelection _levelMap;
   
    private void Start()
    {
        Debug.Log("Ngan - Init Menu");
        _playBtn.onClick.AddListener(OnPlayButtonClicked);

        if (UserProfile.Instance.SelectedBoxIndex != null)
        {
            Debug.Log("Ngan - Selected Box Index: " +UserProfile.Instance.SelectedBoxIndex.Index);

            LoadPreviousBox(UserProfile.Instance.SelectedBoxIndex);

            UserProfile.Instance.SetBoxData(null);
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
        Debug.Log("Ngan - Load previous box");
        _menuPanel.SetActive(false);
        _levelMap.gameObject.SetActive(true);
        _levelMap.LoadLevel(boxData);
    }
}