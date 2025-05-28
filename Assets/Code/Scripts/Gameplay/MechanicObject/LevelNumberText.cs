using System;
using System.Security.Cryptography;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

public class LevelNumberText : MonoBehaviour
{
    [SerializeField] private CanvasGroup _levelTextCanvasGroup;
    [SerializeField] private TMP_Text _levelNumberText;

    private void Start()
    {
        string levelNumber = UserProfile.Instance.SelectedLevelIndex;

        if (levelNumber.Contains("_"))
        {
            levelNumber = levelNumber.Replace("_", "-");
        }

        _levelNumberText.text = levelNumber;

        _levelTextCanvasGroup.alpha = 0f;

        PlayLevelTextAnimation();
    }

    public void PlayLevelTextAnimation()
    {
        LMotion.Create(0f, 1f, 0.3f).WithOnComplete(() =>
        {
            LMotion.Create(1f, 1f, 0.4f).WithOnComplete(() =>
            {
                LMotion.Create(1f, 0f, 0.3f).BindToAlpha(_levelTextCanvasGroup);
            }).BindToAlpha(_levelTextCanvasGroup);
        }).BindToAlpha(_levelTextCanvasGroup);
    }
}