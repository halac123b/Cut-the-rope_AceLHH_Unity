using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private Transform _gridLayoutGroup;
    [SerializeField] private TextMeshProUGUI _numberStar;

    [SerializeField] private Button _backButton;
    [SerializeField] private BoxSelection _boxSelection;

    private void OnEnable()
    {
        _numberStar.text = UserProfile.Instance.GetAllStars().ToString();
    }

    private void Start()
    {
        _backButton.onClick.AddListener(() => OnBackButtonClicked());
    }

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

    private void OnBackButtonClicked()
    {
        Transition.Instance.Appear(Color.black, () =>
        {
            if (_gridLayoutGroup.childCount != 0)
            {
                foreach (Transform child in _gridLayoutGroup)
                {
                    Destroy(child.gameObject);
                }
            }

            gameObject.SetActive(false);
            _boxSelection.gameObject.SetActive(true);
        });
    }
}