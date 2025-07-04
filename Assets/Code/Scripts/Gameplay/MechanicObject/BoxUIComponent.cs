using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxUIComponent : MonoBehaviour
{
    [SerializeField] private Button _btnBox;
    [HideInInspector] public BoxData MyBoxData;
    [SerializeField] private Image _boxSprite;
    [SerializeField] private TMP_Text _boxName;
    
    private void Start()
    {
        _btnBox.onClick.AddListener(() => OnBoxButtonClicked());
    }

    private void OnBoxButtonClicked()
    {
        UserProfile.Instance.SetBoxData(MyBoxData);
        EventDispatcher.Instance.Dispatch(MyBoxData, EventDispatcher.LoadLevelUI);
    }
    
    public void SetUIBoxComponent()
    {
        _boxSprite.sprite = MyBoxData.BoxSprite;
        _boxName.text = MyBoxData.Index.ToString() +". " +MyBoxData.BoxName;
    }


    private void OnDestroy()
    {
        _btnBox.onClick.RemoveListener(() => OnBoxButtonClicked());
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}