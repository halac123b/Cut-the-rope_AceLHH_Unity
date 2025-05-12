using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private Transform _gridLayoutGroup;

    public void LoadLevel(BoxData boxData)
    {
        for (int i = 0; i < boxData.NumberOfLevels; i++)
        {
            GameObject levelObject = Instantiate(_levelPrefab, _gridLayoutGroup);
            LevelObject levelObj = levelObject.GetComponent<LevelObject>();
            levelObj.LevelIndicator = $"{boxData.Index}_{i + 1}";
            levelObj.SetLevelNumber(i);
            int stars = PlayerPrefs.GetInt($"Level_{boxData.Index}_{i + 1}_Stars", -1);

            bool isFirstLevel = (i == 0);
            if (isFirstLevel && stars == -1)
            {
                stars = 0;
                PlayerPrefs.SetInt($"Level_{boxData.Index}_{i + 1}_Stars", stars);
                PlayerPrefs.Save();
            }
            levelObj.SetStars(stars);

            bool isPlayedBefore = stars >= 0;

            if (isFirstLevel || isPlayedBefore)
            {
                levelObj.SetLevelLock(false);
            }
            else
            {
                levelObj.SetLevelLock(true);
            }
        }
    }
}