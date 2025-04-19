using UnityEngine;
using UnityEngine.UI;

public class SeasonSelection : MonoBehaviour
{
    [SerializeField] private Button[] _btnSeason;
    [SerializeField] private GameObject _boxMapPanel;
    [SerializeField] private BoxSelection _boxSelection;

    private void Start()
    {
        for (int i = 0; i < _btnSeason.Length; i++)
        {
            int index = i; // Capture the current index
            _btnSeason[i].onClick.AddListener(() => OnSeasonButtonClicked(index));
        }
    }

    private void OnSeasonButtonClicked(int index)
    {
        gameObject.SetActive(false);
        _boxMapPanel.SetActive(true);
        _boxSelection.LoadBox(index);
    }
}
