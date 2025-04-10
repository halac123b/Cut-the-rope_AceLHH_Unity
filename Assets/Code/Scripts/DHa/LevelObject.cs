using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    public string LevelIndicator;

    private void Start()
    {
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