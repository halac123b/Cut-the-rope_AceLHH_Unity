using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    public StarUI StarUIComponent;
    public CompleteLevelUI CompleteLevelUIComponent;

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, _ => ShowLevelCompleteUI(),
            EventDispatcher.LoadCompleteUI);
    }

    public void ShowLevelCompleteUI()
    {
        StarUIComponent.gameObject.SetActive(false);
        CompleteLevelUIComponent.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}