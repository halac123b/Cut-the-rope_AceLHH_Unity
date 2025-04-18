using UnityEngine;
using UnityEngine.UI;

public class BoxUIComponent : MonoBehaviour
{
    [SerializeField] private Button _btnBox;
    [HideInInspector] public BoxData MyBoxData;

    private void Start()
    {
        _btnBox.onClick.AddListener(() => OnBoxButtonClicked());
    }

    private void OnBoxButtonClicked()
    {
        Debug.Log($"Box {MyBoxData.BoxName} clicked");
        EventDispatcher.Instance.Dispatch(MyBoxData, EventDispatcher.LoadLevelUI);
    }

    private void OnDestroy()
    {
        _btnBox.onClick.RemoveListener(() => OnBoxButtonClicked());
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}