using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private LevelData[] _box1st;
    [SerializeField] private LevelData[] _box2nd;
    [SerializeField] private LevelData[] _box3rd;

    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private Transform _gridLayoutGroup;

    public void LoadLevel(int seasonIndex)
    {
        LevelData[] levelList = null;
        switch (seasonIndex)
        {
            case 0:
                levelList = _box1st;
                break;
            case 1:
                levelList = _box2nd;
                break;
            case 2:
                levelList = _box3rd;
                break;
        }
        
        for (int i = 0; i < levelList.Length; i++)
        {
            GameObject levelObject = Instantiate(_levelPrefab, _gridLayoutGroup);
            LevelObject levelObj = levelObject.GetComponent<LevelObject>();
            levelObj.LevelIndicator = levelList[i].levelName;

            int stars = PlayerPrefs.GetInt($"Level_{levelList[i].levelName}_Stars", 0);
            levelObj.SetStars(stars);
        }
    }
}
