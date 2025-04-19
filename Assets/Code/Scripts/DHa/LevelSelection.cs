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
            levelObj.LevelIndicator = (i + 1).ToString();

            int stars = PlayerPrefs.GetInt($"Level_{boxData.Index}_{i + 1}_Stars", 0);
            levelObj.SetStars(stars);
        }
    }
}