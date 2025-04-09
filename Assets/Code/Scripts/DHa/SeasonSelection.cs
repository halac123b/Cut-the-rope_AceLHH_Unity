using UnityEngine;
using UnityEngine.UI;

public class SeasonSelection : MonoBehaviour
{
    [SerializeField] private Button[] _btnSeason;
    [SerializeField] private GameObject _levelMapPanel;
    [SerializeField] private LevelSelection _levelSelection;

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
        _levelMapPanel.SetActive(true);

        _levelSelection.LoadLevel(index);
    }
}
