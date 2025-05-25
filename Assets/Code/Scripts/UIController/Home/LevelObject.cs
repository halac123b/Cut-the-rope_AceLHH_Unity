using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private TMP_Text _tmpStars;
    [SerializeField] private Image _levelLockUI;
    public TMP_Text LevelText;
    public List<Sprite> StarLevelSprites;
    public Image StarLevelImage;
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

    public void SetLevelNumber(int levelIndex)
    {
        LevelText.text = (levelIndex + 1).ToString();
    }

    public void SetStars(int starCount)
    {
        if (starCount < 0)
        {
            _tmpStars.text = "0";
        }
        else
        {
            _tmpStars.text = starCount.ToString();
        }

        int spriteIndex = Mathf.Clamp(starCount, 0, StarLevelSprites.Count - 1);

        if (StarLevelSprites != null && StarLevelSprites.Count > spriteIndex)
        {
            StarLevelImage.sprite = StarLevelSprites[spriteIndex];
        }
    }

    public void SetLevelLock(bool isLocked)
    {
        _levelLockUI.gameObject.SetActive(isLocked);
        _btnPlay.gameObject.SetActive(!isLocked);
    }
}