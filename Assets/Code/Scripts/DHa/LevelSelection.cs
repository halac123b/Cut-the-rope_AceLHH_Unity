using UnityEngine;
using System;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private LevelData[] _season1st;
    [SerializeField] private LevelData[] _season2nd;
    [SerializeField] private LevelData[] _season3rd;

    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private Transform _gridLayoutGroup;

    private void OnEnable()
    {
        CompleteLevelUI.OnLevelComplete += LoadLevelFromGamplay;
    }

    private void OnDestroy()
    {
        CompleteLevelUI.OnLevelComplete -= LoadLevelFromGamplay;
    }

    public void LoadLevelFromGamplay()
    {
        gameObject.SetActive(true);
        //LoadLevel();
    }

    public void LoadLevel(int seasonIndex)
    {
        LevelData[] levelList = null;
        switch (seasonIndex)
        {
            case 0:
                levelList = _season1st;
                break;
            case 1:
                levelList = _season2nd;
                break;
            case 2:
                levelList = _season3rd;
                break;
        }
        for (int i = 0; i < levelList.Length; i++)
        {
            GameObject levelObject = Instantiate(_levelPrefab, _gridLayoutGroup);
            string levelName = levelList[i].levelName;
            
            levelObject.GetComponent<LevelObject>().LevelIndicator = levelName;
            
            int stars = PlayerPrefs.GetInt($"Level_{levelName}_Stars", 0);
            levelObject.GetComponent<LevelObject>().SetStars(stars); 
        }
    }
}
