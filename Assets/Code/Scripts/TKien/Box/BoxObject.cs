using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BoxObject : MonoBehaviour
{
    [SerializeField] private Button[] _btnBox;
    [SerializeField] private GameObject _boxMapPanel;
    [SerializeField] private BoxSelection _boxSelection;

    private void Start()
    {
        for (int i = 0; i < _btnBox.Length; i++)
        {
            int index = i; // Capture the current index
            _btnBox[i].onClick.AddListener(() => OnBoxButtonClicked(index));
        }
    }

    private void OnBoxButtonClicked(int index)
    {
        gameObject.SetActive(false);
        _boxMapPanel.SetActive(true);
        _boxSelection.LoadBox(index);
    }

}
