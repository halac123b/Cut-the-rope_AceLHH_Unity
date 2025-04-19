using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoSingleton<UIController>
{
    public StarUI StarUIComponent;
    public CompleteLevelUI CompleteLevelUIComponent;

    public void ShowLevelCompleteUI()
    {
        StarUIComponent.gameObject.SetActive(false);
        CompleteLevelUIComponent.gameObject.SetActive(true);
    }
}
