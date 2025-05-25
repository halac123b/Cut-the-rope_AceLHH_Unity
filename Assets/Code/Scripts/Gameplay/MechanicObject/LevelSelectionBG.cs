using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelSelectionBG : MonoBehaviour
{
    [SerializeField] private Image _levelImageRight;
    [SerializeField] private Image _levelImageLeft;

    private void Start()
    {
        _levelImageRight.sprite = UserProfile.Instance.SelectedBoxIndex.BoxBGSprite;
        _levelImageLeft.sprite = UserProfile.Instance.SelectedBoxIndex.BoxBGSprite;
    }
}