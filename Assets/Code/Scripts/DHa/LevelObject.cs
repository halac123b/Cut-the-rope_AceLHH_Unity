using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private TMP_Text _tmpText;
    public string LevelIndicator;

    private void Start()
    {
        if (_tmpText != null)
        {
            _tmpText.text = LevelIndicator;
        }
        else
        {
            Debug.LogWarning("[Kien],[LevelItem] TMP_Text reference is missing.");
        }
        _btnPlay.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        UserProfile.Instance.SetLevel(LevelIndicator);
        Debug.Log($"Load game Level {LevelIndicator}.");
        //SceneManager.LoadScene("GamePlay");
        
        //Kien's scene Test
        SceneManager.LoadScene("KienTest");
    }
}