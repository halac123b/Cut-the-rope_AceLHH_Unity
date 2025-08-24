using UnityEngine;
using UnityEngine.UI;

public class SeasonSelection : MonoBehaviour
{
    [SerializeField] private Button[] _btnSeason;
    [SerializeField] private Button _btnBack;
    [SerializeField] private GameObject _introScreen;
    [SerializeField] private GameObject _boxMapPanel;
    [SerializeField] private BoxSelection _boxSelection;

    private void Start()
    {
        for (int i = 0; i < _btnSeason.Length; i++)
        {
            int index = i; // Capture the current index
            _btnSeason[i].onClick.AddListener(() => OnSeasonButtonClicked(index));
        }

        _btnBack.onClick.AddListener(() => OnBackButtonClicked());
    }

    private void OnSeasonButtonClicked(int index)
    {
        UserProfile.Instance.SeasonIndex = index;
        
        Transition.Instance.Appear(Color.black, () =>
        {
            gameObject.SetActive(false);
            _boxMapPanel.SetActive(true);
        });
    }

    private void OnBackButtonClicked()
    {
        Transition.Instance.Appear(Color.black, () =>
        {
            gameObject.SetActive(false);
            _introScreen.SetActive(true);
        });
    }
}