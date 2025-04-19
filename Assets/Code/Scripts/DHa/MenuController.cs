using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _playBtn;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _seasonPanel;

    [SerializeField] private GameObject _levelMap;

    private void Start()
    {
        _playBtn.onClick.AddListener(OnPlayButtonClicked);

        if (!GameManager.Instance.CurrentBox.IsNullOrEmpty())
        {
            LoadPreviousBox();

            GameManager.Instance.CurrentBox = "";
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
    }

    private void LoadPreviousBox()
    {
        _levelMap.SetActive(true);
        _menuPanel.SetActive(false);
    }
}
