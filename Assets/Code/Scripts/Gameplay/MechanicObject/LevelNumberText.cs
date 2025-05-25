using System;
using TMPro;
using UnityEngine;

public class LevelNumberText : MonoBehaviour
{
    [SerializeField] TMP_Text _levelNumberText;

    private void Start()
    {
        string levelNumber = UserProfile.Instance.SelectedLevelIndex;
        
        if (levelNumber.Contains("_"))
        {
            levelNumber = levelNumber.Replace("_", "-");
        }

        _levelNumberText.text = levelNumber;
    }
}