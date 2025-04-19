using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private TMP_Text _tmpStars;
    public string LevelIndicator;

    private void Start()
    {
        _tmpText.text = LevelIndicator;
        _btnPlay.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        UserProfile.Instance.SetLevel(LevelIndicator);
        GameManager.Instance.CurrentBox = LevelIndicator;
        Debug.Log($"Load game Level {LevelIndicator}.");
        SceneManager.LoadScene("GamePlay");

        //Kien's scene Test
        //SceneManager.LoadScene("KienTest");
    }
    
    public void SetStars(int starCount)
    {
        _tmpStars.text = $"{starCount}";
    }

}