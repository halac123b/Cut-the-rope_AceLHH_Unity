using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private SeasonData _season1st;
    [SerializeField] private SeasonData _season2nd;
    [SerializeField] private SeasonData _season3rd;

    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private Transform _gridLayoutGroup;
    [SerializeField] private LevelSelection _levelSelection;

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, boxData =>
        {
            LoadLevel((BoxData)boxData);
        }, EventDispatcher.LoadLevelUI);
    }

    public void LoadBox(int seasonIndex)
    {
        BoxData[] boxlList = null;
        switch (seasonIndex)
        {
            case 0:
                boxlList = _season1st.BoxList;
                break;
            case 1:
                boxlList = _season2nd.BoxList;
                break;
            case 2:
                boxlList = _season3rd.BoxList;
                break;
        }

        for (int i = 0; i < boxlList.Length; i++)
        {
            GameObject boxGameObject = Instantiate(_boxPrefab, _gridLayoutGroup);
            boxGameObject.GetComponent<BoxUIComponent>().MyBoxData = boxlList[i];
        }
    }

    private void LoadLevel(BoxData boxData)
    {
        _levelSelection.gameObject.SetActive(true);
        _levelSelection.LoadLevel(boxData);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}