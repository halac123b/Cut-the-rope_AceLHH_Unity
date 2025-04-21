using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private TMP_Text _tmpStars;
    [SerializeField] private Image _levelLockUI;
    public string LevelIndicator;

    private void Start()
    {
        _tmpText.text = LevelIndicator.Split('_')[1];
        _btnPlay.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        UserProfile.Instance.SetLevel(LevelIndicator);
        Debug.Log($"Load game Level {LevelIndicator}.");
        
        SceneManager.LoadScene("GamePlay");

        //Kien's scene Test
        //SceneManager.LoadScene("KienTest");
    }

    public void SetStars(int starCount)
    {
        _tmpStars.text = $"{starCount}";
    }

    public void SetLevelLock(bool isLocked)
    {
        _levelLockUI.gameObject.SetActive(isLocked);
        _btnPlay.interactable = !isLocked;
    }
}